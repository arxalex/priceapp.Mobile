using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Annotations;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: Xamarin.Forms.Dependency(typeof(ItemViewModel))]

namespace priceapp.ViewModels;

public class ItemViewModel : IItemViewModel
{
    private readonly IBrandAlertRepository _brandAlertRepository = DependencyService.Get<IBrandAlertRepository>();
    private readonly ILocationService _locationService = DependencyService.Get<ILocationService>();
    private readonly IItemRepository _itemRepository = DependencyService.Get<IItemRepository>();
    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();
    private readonly IShopRepository _shopRepository = DependencyService.Get<IShopRepository>();
    private readonly IMapper _mapper = DependencyService.Get<IMapper>();
    private BrandAlert _brandAlert;
    private Color _foreGroundColorBrandAlert;
    private bool _isVisibleBrandAlert;

    private Item _item;

    public ItemViewModel()
    {
        _itemRepository.BadConnectEvent += ItemRepositoryOnBadConnectEvent;

        PricesAndFilials = new ObservableCollection<ItemPriceInfo>();
    }

    public event ConnectionErrorHandler BadConnectEvent;
    public event LoadingHandler Loaded;

    public Item Item
    {
        get => _item;
        set
        {
            _item = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<ImageButtonModel> ItemButtons { get; set; } = new();

    public ObservableCollection<ItemPriceInfo> PricesAndFilials { get; set; }

    public BrandAlert BrandAlert
    {
        get => _brandAlert;
        set
        {
            _brandAlert = value;
            IsVisibleBrandAlert = BrandAlert is { Message.Length: > 0 };
            ForeGroundColorBrandAlert = ColorUtil.BlackOrWhiteFrontColorByBackground(BrandAlert.Color);
            OnPropertyChanged();
        }
    }

    public Color ForeGroundColorBrandAlert
    {
        get => _foreGroundColorBrandAlert;
        set
        {
            _foreGroundColorBrandAlert = value;
            OnPropertyChanged();
        }
    }

    public bool IsVisibleBrandAlert
    {
        get => _isVisibleBrandAlert;
        set
        {
            _isVisibleBrandAlert = value;
            OnPropertyChanged();
        }
    }

    public async Task LoadAsync(Item item, Page page)
    {
        var shops = _mapper.Map<IList<Shop>>(await _shopRepository.GetShops());
        var filials = _mapper.Map<IList<Filial>>(await _shopRepository.GetFilials());
        var location = await _locationService.GetLocationAsync();
        Item = item;

        var priceInfos = _mapper.Map<IList<PriceModel>>(await _itemRepository
            .GetPricesAndFilials(
                Item.Id,
                location.Longitude,
                location.Latitude,
                Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius)
            ));

        priceInfos.Select(x => new ItemPriceInfo()
        {
            Price = x.Price,
            ItemId = x.ItemId,
            Quantity = x.Quantity,
            Filial = filials.Last(f => f.Id == x.FilialId),
            Shop = shops.Last(s => s.Id == x.ShopId)
        }).ForEach(x => { PricesAndFilials.Add(x); });
        priceInfos.Select(x =>
        {
            var shop = shops.Last(s => s.Id == x.ShopId);
            var filial = filials.Last(f => f.Id == x.FilialId);
            return new ImageButtonModel()
            {
                Id = x.Id,
                Image = shop.Icon,
                PrimaryText = shop.Label,
                SecondaryText = filial.Street + " " + filial.House,
                AdditionalText = x.Quantity > 0 ? "Є в наявності" : "Немає в наявності",
                AdditionalTextColor = x.Quantity > 0 ? (Color)Application.Current.Resources["ColorPrimary"] : Color.Red,
                AccentText = x.Price + " грн",
                Command = new Command(async () =>
                {
                    const string addAction = "Додати до кошика";
                    var action = await page.DisplayActionSheet("Дії:", "Закрити", null, addAction);
                    if (action == addAction)
                    {
                        await AddToCart(filial.Id);
                    }
                })
            };
        }).ForEach(x => ItemButtons.Add(x));

        if (Xamarin.Essentials.Preferences.Get("showRussiaSupportBrandAlerts",
                Constants.DefaultShowRussiaSupportBrandAlerts))
        {
            var alerts = await _brandAlertRepository.GetBrandAlerts(Item.Brand);
            foreach (var alert in alerts)
            {
                if (alert.color != "#ff0000") continue;
                BrandAlert = _mapper.Map<BrandAlert>(alert);
                break;
            }
        }

        Loaded?.Invoke(this, new LoadingArgs()
        {
            Success = true,
            LoadedCount = 1,
            Total = 1
        });
    }

    public async Task AddToCart(int? filialId = null)
    {
        var itemToBuy = new ItemToBuy()
        {
            Added = false,
            Count = 1,
            Filial = filialId != null ? new Filial { Id = (int)filialId } : null,
            Item = Item
        };
        await _itemsToBuyLocalRepository.InsertAsync(_mapper.Map<ItemToBuyLocalDatabaseModel>(itemToBuy));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
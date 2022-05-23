using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Annotations;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(ItemViewModel))]

namespace priceapp.ViewModels;

public class ItemViewModel : IItemViewModel
{
    private readonly GeolocationUtil _geolocationUtil;
    private readonly IItemRepository _itemRepository;
    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository;
    private readonly IMapper _mapper;

    private Item _item;

    public ItemViewModel()
    {
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _geolocationUtil = DependencyService.Get<GeolocationUtil>();
        _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();

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

    public ObservableCollection<ItemPriceInfo> PricesAndFilials { get; set; }

    public async Task LoadAsync(Item item)
    {
        var location = await _geolocationUtil.GetCurrentLocation();
        Item = item;

        var priceInfos = await _itemRepository
            .GetPricesAndFilials(
                Item.Id,
                location.Longitude,
                location.Latitude,
                Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius)
            );

        foreach (var priceInfo in priceInfos)
        {
            PricesAndFilials.Add(_mapper.Map<ItemPriceInfo>(priceInfo));
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
            Filial = filialId != null ? new Filial {Id = (int) filialId} : null,
            Item = Item
        };
        await _itemsToBuyLocalRepository.AddItem(_mapper.Map<ItemToBuyLocalDatabaseModel>(itemToBuy));
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
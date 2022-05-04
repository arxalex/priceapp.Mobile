using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Annotations;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
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
    public event ConnectionErrorHandler BadConnectEvent;
    public event LoadingHandler Loaded;
    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;
    private readonly GeolocationUtil _geolocationUtil;

    private Item _item;

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

    public ItemViewModel()
    {
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _geolocationUtil = DependencyService.Get<GeolocationUtil>();
        
        _itemRepository.BadConnectEvent += ItemRepositoryOnBadConnectEvent;

        PricesAndFilials = new ObservableCollection<ItemPriceInfo>();
    }

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    public async Task LoadAsync(Item item)
    {
        var location = await _geolocationUtil.GetCurrentLocation();
        Item = item;

        var priceInfos = await _itemRepository
            .GetPricesAndFilials(
                Item.Id,
                location.Longitude,
                location.Latitude,
                Xamarin.Essentials.Preferences.Get("locationRadius", 5000)
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

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
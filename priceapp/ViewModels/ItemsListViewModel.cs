using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ItemsListViewModel))]

namespace priceapp.ViewModels;

public class ItemsListViewModel : IItemsListViewModel
{
    public event LoadingHandler Loaded;
    public event ConnectionErrorHandler BadConnectEvent;

    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;
    private readonly GeolocationUtil _geolocationUtil;
    private const int PageSize = 20;
    public ObservableCollection<Item> Items { get; set; }
    public int CategoryId { get; set; }
    private bool CanLoadMode { get; set; }

    public ItemsListViewModel()
    {
        Items = new ObservableCollection<Item>();
        CanLoadMode = true;
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _geolocationUtil = DependencyService.Get<GeolocationUtil>();
        
        _itemRepository.BadConnectEvent += ItemRepositoryOnBadConnectEvent;
    }

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    public async Task LoadAsync()
    {
        if (!CanLoadMode)
        {
            Loaded?.Invoke(this, new LoadingArgs(){Success = false, Total = Items.Count});
            return;
        }

        var location = await _geolocationUtil.GetCurrentLocation();

        var items = await _itemRepository
            .GetItems(
                CategoryId,
                Items.Count,
                Items.Count + PageSize,
                location.Longitude,
                location.Latitude,
                Xamarin.Essentials.Preferences.Get("locationRadius", 5000)
            );

        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                Items.Add(_mapper.Map<Item>(item));
            }
        }

        CanLoadMode = items.Count >= PageSize;
        Loaded?.Invoke(this, new LoadingArgs(){Success = true, Total = Items.Count, LoadedCount = items.Count});
    }
}
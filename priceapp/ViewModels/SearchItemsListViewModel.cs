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

[assembly: Dependency(typeof(SearchItemsListViewModel))]

namespace priceapp.ViewModels;

public class SearchItemsListViewModel : ISearchItemsListViewModel
{
    private const int PageSize = 20;
    private readonly GeolocationUtil _geolocationUtil;
    private readonly IItemRepository _itemRepository;

    private readonly IMapper _mapper;

    public SearchItemsListViewModel()
    {
        Items = new ObservableCollection<Item>();
        CanLoadMode = true;
        ItemsLoadingNow = false;
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _geolocationUtil = DependencyService.Get<GeolocationUtil>();

        _itemRepository.BadConnectEvent += ItemRepositoryOnBadConnectEvent;
    }

    private bool CanLoadMode { get; set; }
    private bool ItemsLoadingNow { get; set; }
    public event LoadingHandler Loaded;
    public event ConnectionErrorHandler BadConnectEvent;
    public ObservableCollection<Item> Items { get; set; }
    public string Search { get; set; }

    public async Task LoadAsync()
    {
        if (ItemsLoadingNow)
        {
            Loaded?.Invoke(this, new LoadingArgs() {Success = false, Total = Items.Count});
            return;
        }

        ItemsLoadingNow = true;

        if (!CanLoadMode)
        {
            ItemsLoadingNow = false;
            Loaded?.Invoke(this, new LoadingArgs() {Success = false, Total = Items.Count});
            return;
        }

        var location = await _geolocationUtil.GetCurrentLocation();

        var items = await _itemRepository
            .SearchItems(
                Search,
                Items.Count,
                Items.Count + PageSize,
                location.Longitude,
                location.Latitude,
                Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius)
            );

        if (items.Count > 0)
        {
            foreach (var item in items)
            {
                Items.Add(_mapper.Map<Item>(item));
            }
        }

        CanLoadMode = items.Count >= PageSize;
        ItemsLoadingNow = false;
        Loaded?.Invoke(this, new LoadingArgs() {Success = true, Total = Items.Count, LoadedCount = items.Count});
    }

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }
}
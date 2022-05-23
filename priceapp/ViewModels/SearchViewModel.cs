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

[assembly: Dependency(typeof(SearchViewModel))]

namespace priceapp.ViewModels;

public class SearchViewModel : ISearchViewModel
{
    private const int PageSize = 5;
    private readonly GeolocationUtil _geolocationUtil;
    private readonly IItemRepository _itemRepository;

    private readonly IMapper _mapper;

    public SearchViewModel()
    {
        Items = new ObservableCollection<Item>();
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _geolocationUtil = DependencyService.Get<GeolocationUtil>();

        _itemRepository.BadConnectEvent += ItemRepositoryOnBadConnectEvent;
    }

    public event LoadingHandler Loaded;
    public event ConnectionErrorHandler BadConnectEvent;
    public ObservableCollection<Item> Items { get; set; }
    public string Search { get; set; }

    public async Task LoadAsync()
    {
        Items.Clear();

        var location = await _geolocationUtil.GetCurrentLocation();

        var items = await _itemRepository
            .SearchItems(
                Search,
                0,
                PageSize,
                location.Longitude,
                location.Latitude,
                Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius)
            );


        foreach (var item in items)
        {
            Items.Add(_mapper.Map<Item>(item));
        }

        Loaded?.Invoke(this, new LoadingArgs() {Success = true, Total = Items.Count, LoadedCount = items.Count});
    }

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }
}
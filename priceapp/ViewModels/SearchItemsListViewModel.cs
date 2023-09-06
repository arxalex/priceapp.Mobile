using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

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
    public string Search { get; set; }

    public ObservableCollection<ImageButtonModel> ItemButtons { get; set; } = new();

    public async Task LoadAsync(INavigation navigation)
    {
        if (ItemsLoadingNow)
        {
            Loaded?.Invoke(this, new LoadingArgs() {Success = false, Total = ItemButtons.Count});
            return;
        }

        ItemsLoadingNow = true;

        if (!CanLoadMode)
        {
            ItemsLoadingNow = false;
            Loaded?.Invoke(this, new LoadingArgs() {Success = false, Total = ItemButtons.Count});
            return;
        }

        var location = await _geolocationUtil.GetCurrentLocation();

        var items = await _itemRepository
            .SearchItems(
                Search,
                ItemButtons.Count,
                ItemButtons.Count + PageSize,
                location.Longitude,
                location.Latitude,
                Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius)
            );

        if (items.Count > 0)
        {
            items.Select(y =>
            {
                var x = _mapper.Map<Item>(y);
                return new ImageButtonModel()
                {
                    Id = x.Id,
                    Image = x.Image,
                    PrimaryText = x.Label,
                    SecondaryText = x.UnitsText,
                    AccentText = x.PriceText,
                    Command = new Command(async () => { await navigation.PushAsync(new ItemPage(x)); })
                };
            }).ForEach(x => ItemButtons.Add(x));
        }

        CanLoadMode = items.Count >= PageSize;
        ItemsLoadingNow = false;
        Loaded?.Invoke(this, new LoadingArgs() {Success = true, Total = ItemButtons.Count, LoadedCount = items.Count});
    }

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }
}
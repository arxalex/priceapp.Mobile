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
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _geolocationUtil = DependencyService.Get<GeolocationUtil>();

        _itemRepository.BadConnectEvent += ItemRepositoryOnBadConnectEvent;
    }

    public event LoadingHandler Loaded;
    public event ConnectionErrorHandler BadConnectEvent;
    public string Search { get; set; }
    public ObservableCollection<ImageButtonModel> ItemButtons { get; set; } = new();

    public async Task LoadAsync(INavigation navigation)
    {
        ItemButtons.Clear();

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

        Loaded?.Invoke(this, new LoadingArgs() {Success = true, Total = ItemButtons.Count, LoadedCount = items.Count});
    }

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }
}
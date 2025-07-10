using System.Collections.ObjectModel;
using AutoMapper;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp.ViewModels;

public class SearchViewModel : ISearchViewModel
{
    private const int PageSize = 5;
    private readonly ILocationService _locationService;
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;
    private readonly IServiceProvider _serviceProvider;

    public SearchViewModel(
        ILocationService locationService,
        IItemRepository itemRepository, 
        IMapper mapper, IServiceProvider serviceProvider) {
        _locationService = locationService;
        _itemRepository = itemRepository;
        _mapper = mapper;
        _serviceProvider = serviceProvider;
        _itemRepository.BadConnectEvent += ItemRepositoryOnBadConnectEvent;
    }

    public event LoadingHandler? Loaded;
    public event ConnectionErrorHandler? BadConnectEvent;
    public string? Search { get; set; }
    public ObservableCollection<ImageButtonModel> ItemButtons { get; set; } = new();

    public async Task LoadAsync(INavigation navigation)
    {
        ItemButtons.Clear();

        var location = await _locationService.GetLocationAsync();

        var items = await _itemRepository
            .SearchItems(
                Search,
                0,
                PageSize,
                location.Longitude,
                location.Latitude,
                Preferences.Get("locationRadius", Constants.DefaultRadius)
            );


        items.Select(y =>
        {
            var x = _mapper.Map<Item>(y);
            return new ImageButtonModel
            {
                Id = x.Id,
                Image = x.Image,
                PrimaryText = x.Label,
                SecondaryText = x.UnitsText,
                AccentText = x.PriceText,
                Command = new Command(() => { navigation.PushAsync(new ItemPage(x, _serviceProvider.GetRequiredService<IItemViewModel>(), _serviceProvider.GetRequiredService<ILocationService>())); })
            };
        }).ForEach(x => ItemButtons.Add(x));

        Loaded?.Invoke(this, new LoadingArgs {Success = true, Total = ItemButtons.Count, LoadedCount = items.Count});
    }

    private void ItemRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }
}
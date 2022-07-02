using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using priceapp.Annotations;
using priceapp.Enums;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: Xamarin.Forms.Dependency(typeof(CartViewModel))]

namespace priceapp.ViewModels;

public class CartViewModel : ICartViewModel
{
    private const int RefreshDuration = 0;
    private readonly GeolocationUtil _geolocationUtil;
    private readonly IItemRepository _itemRepository;

    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository;
    private readonly IMapper _mapper;
    private readonly IShopRepository _shopRepository;
    private string _economy;

    private string _headerText;
    private bool _isRefreshing;
    private ObservableCollection<ItemToBuyGroup> _itemsToBuyList = new();

    public CartViewModel()
    {
        _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();
        _shopRepository = DependencyService.Get<IShopRepository>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _geolocationUtil = DependencyService.Get<GeolocationUtil>();
        _mapper = DependencyService.Get<IMapper>();
        _economy = "0.00 грн";

        _itemsToBuyLocalRepository.BadConnectEvent += ItemsToBuyLocalRepositoryOnBadConnectEvent;
        _itemRepository.BadConnectEvent += ItemsToBuyLocalRepositoryOnBadConnectEvent;
    }

    public event LoadingHandler Loaded;
    public event ConnectionErrorHandler BadConnectEvent;

    public bool IsRefreshing
    {
        get => _isRefreshing;
        set
        {
            _isRefreshing = value;
            OnPropertyChanged();
        }
    }

    public ICommand RefreshCommand => new Command(async () =>
    {
        IsRefreshing = true;
        await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
        await LoadAsync();
        IsRefreshing = false;
    });

    public ObservableCollection<ItemToBuyGroup> ItemsToBuyList
    {
        get => _itemsToBuyList;
        set
        {
            _itemsToBuyList = value;
            OnPropertyChanged();
        }
    }

    public string Economy
    {
        get => _economy;
        set
        {
            _economy = value;
            OnPropertyChanged();
        }
    }

    public string HeaderText
    {
        get => _headerText;
        set
        {
            _headerText = value;
            OnPropertyChanged();
        }
    }

    public List<ItemToBuy> ItemsToBuyListPreProcessed { get; set; }

    public async Task LoadAsync()
    {
        ItemsToBuyListPreProcessed = new List<ItemToBuy>();
        ItemsToBuyList = new ObservableCollection<ItemToBuyGroup>();
        var items = await _itemsToBuyLocalRepository.GetItems();
        var filials = await _shopRepository.GetFilials();
        var shops = await _shopRepository.GetShops();

        if (items.Count < 1)
        {
            Economy = "0.00 грн";
            HeaderText = "Доступно 0 з " + ItemsToBuyListPreProcessed.Count;
            Loaded?.Invoke(this,
                new LoadingArgs()
                    {Success = true, LoadedCount = items.Count, Total = ItemsToBuyListPreProcessed.Count});
            return;
        }

        foreach (var item in items)
        {
            var itemToBuy = _mapper.Map<ItemToBuy>(item);
            itemToBuy.Item = _mapper.Map<Item>(await _itemRepository.GetItem(item.ItemId));
            if (item.FilialId != null)
            {
                itemToBuy.Filial =
                    _mapper.Map<Filial>(filials.Last(x => x.id == item.FilialId));
                itemToBuy.Filial.Shop =
                    _mapper.Map<Shop>(shops.Last(x => x.id == itemToBuy.Filial.Shop.Id));
            }

            ItemsToBuyListPreProcessed.Add(itemToBuy);
        }

        var location = await _geolocationUtil.GetCurrentLocation();
        var (itemsResult, economy) = await _itemRepository.GetShoppingList(
            _mapper.Map<List<ItemToBuyRepositoryModel>>(ItemsToBuyListPreProcessed),
            location.Longitude, location.Latitude,
            Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius),
            (CartProcessingType) Xamarin.Essentials.Preferences.Get("cartProcessingType",
                (int) CartProcessingType.MultipleMarketsLowest));

        if (itemsResult.Count == 0)
        {
            Economy = "0.00 грн";
            HeaderText = "Доступно 0 з " + ItemsToBuyListPreProcessed.Count;
            Loaded?.Invoke(this,
                new LoadingArgs()
                    {Success = false, LoadedCount = items.Count, Total = ItemsToBuyListPreProcessed.Count});
            return;
        }

        Economy = Math.Round(economy, 2) + " грн";

        var itemsToBuyListUngrouped = new List<ItemToBuy>();

        foreach (var item in ItemsToBuyListPreProcessed)
        {
            PriceAndFilialRepositoryModel itemResult;
            if (item.Filial == null)
            {
                itemResult = itemsResult.First(x => x.itemId == item.Item.Id);
            }
            else
            {
                itemResult = itemsResult.First(x => x.itemId == item.Item.Id && x.filialId == item.Filial.Id);
            }

            var itemToBuy = new ItemToBuy()
            {
                RecordId = item.RecordId,
                Added = true,
                Count = item.Count,
                Filial = _mapper.Map<Filial>(filials.Last(x => x.id == itemResult.filialId)),
                Item = item.Item
            };

            itemToBuy.Item.PriceMin = itemResult.price;
            itemToBuy.Item.PriceMax = itemResult.price;

            itemsToBuyListUngrouped.Add(itemToBuy);
        }

        itemsToBuyListUngrouped.GroupBy(x =>
                x.Filial.City + ", " + x.Filial.Street + " " + x.Filial.House + ", " +
                shops.Last(y => y.id == x.Filial.Shop.Id).label)
            .ForEach(x => { ItemsToBuyList.Add(new ItemToBuyGroup(x.Key, x.ToList())); });

        HeaderText = "Доступно " + itemsToBuyListUngrouped.Count + " з " + ItemsToBuyListPreProcessed.Count;

        Loaded?.Invoke(this,
            new LoadingArgs() {Success = true, LoadedCount = items.Count, Total = ItemsToBuyListPreProcessed.Count});
    }

    public async Task ChangeCartItem(ItemToBuy model)
    {
        await _itemsToBuyLocalRepository.UpdateItem(_mapper.Map<ItemToBuyLocalDatabaseModel>(model));
        await LoadAsync();
    }

    public async Task DeleteCartItem(ItemToBuy model)
    {
        await _itemsToBuyLocalRepository.RemoveItem(model.RecordId);
        await LoadAsync();
    }

    public async Task ClearShoppingList()
    {
        ItemsToBuyList = new ObservableCollection<ItemToBuyGroup>();
        ItemsToBuyListPreProcessed = new List<ItemToBuy>();
        HeaderText = "Доступно 0 з 0";
        await _itemsToBuyLocalRepository.RemoveAll();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void ItemsToBuyLocalRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
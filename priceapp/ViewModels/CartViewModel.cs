using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using priceapp.Annotations;
using priceapp.Controls.Models;
using priceapp.Enums;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

[assembly: Xamarin.Forms.Dependency(typeof(CartViewModel))]

namespace priceapp.ViewModels;

public class CartViewModel : ICartViewModel
{
    private const int RefreshDuration = 0;
    private readonly ILocationService _locationService = DependencyService.Get<ILocationService>();
    private readonly IItemRepository _itemRepository = DependencyService.Get<IItemRepository>();

    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();
    private readonly IMapper _mapper = DependencyService.Get<IMapper>();
    private readonly IShopRepository _shopRepository = DependencyService.Get<IShopRepository>();
    private string _economy;

    private string _headerText;
    private bool _isRefreshing;
    private ObservableCollection<ImageButtonsGroup> _imageButtons = new();

    public CartViewModel()
    {
        _economy = "0.00 грн";

        _itemsToBuyLocalRepository.BadConnectEvent += ItemsToBuyLocalRepositoryOnBadConnectEvent;
        _itemRepository.BadConnectEvent += ItemsToBuyLocalRepositoryOnBadConnectEvent;
        _shopRepository.BadConnectEvent += ItemsToBuyLocalRepositoryOnBadConnectEvent;
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

    public ObservableCollection<ImageButtonsGroup> ImageButtons
    {
        get => _imageButtons;
        set
        {
            _imageButtons = value;
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

    public Page Page { get; set; }

    public string HeaderText
    {
        get => _headerText;
        set
        {
            _headerText = value;
            OnPropertyChanged();
        }
    }

    public async Task LoadAsync()
    {
        ImageButtons = new ObservableCollection<ImageButtonsGroup>();
        var items = await _itemsToBuyLocalRepository.GetAsync();
        var filials = await _shopRepository.GetFilials();
        var shops = await _shopRepository.GetShops();

        if (items.Count < 1)
        {
            Economy = "0.00 грн";
            HeaderText = "Доступно 0 з 0";
            Loaded?.Invoke(this,
                new LoadingArgs()
                    { Success = true, LoadedCount = items.Count, Total = 0 });
            return;
        }

        var itemsToBuyListPreProcessed = new List<ItemToBuy>();
        
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

            itemsToBuyListPreProcessed.Add(itemToBuy);
        }

        var location = await _locationService.GetLocationAsync();
        var (itemsResult, economy) = await _itemRepository.GetShoppingList(
            _mapper.Map<List<ShoppingListRepositoryModel>>(itemsToBuyListPreProcessed),
            location.Longitude, location.Latitude,
            Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius),
            (CartProcessingType)Xamarin.Essentials.Preferences.Get("cartProcessingType",
                (int)CartProcessingType.MultipleMarketsLowest));

        if (itemsResult.Count == 0)
        {
            Economy = "0.00 грн";
            HeaderText = "Доступно 0 з " + itemsToBuyListPreProcessed.Count;
            Loaded?.Invoke(this,
                new LoadingArgs()
                    { Success = false, LoadedCount = items.Count, Total = itemsToBuyListPreProcessed.Count });
            return;
        }

        Economy = Math.Round(economy, 2) + " грн";

        var itemsToBuyListUngrouped = new List<ItemToBuy>();

        foreach (var item in itemsToBuyListPreProcessed)
        {
            PriceRepositoryModel itemResult;
            if (item.Filial == null)
            {
                itemResult = itemsResult.FirstOrDefault(x => x.itemId == item.Item.Id);
            }
            else
            {
                itemResult = itemsResult.FirstOrDefault(x => x.itemId == item.Item.Id && x.filialId == item.Filial.Id);
            }

            if (itemResult == null)
            {
                continue;
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
        
        var query = from item in itemsToBuyListUngrouped
            group item by item.Filial.City + ", " + item.Filial.Street + " " + item.Filial.House + ", " +
            shops.Last(y => y.id == item.Filial.Shop.Id).label into grouped
            select new
            {
                grouped.Key,
                Result = grouped.Select(x =>
                {
                    return new ImageButtonModel()
                    {
                        Id = x.RecordId,
                        Image = x.Item.Image,
                        PrimaryText = x.Item.Label,
                        SecondaryText = x.CountLabel,
                        AccentText = x.Item.PriceText,
                        Command = new Command(async () =>
                        {
                            const string changeQuantity = "Змінити кількість";
                            const string delete = "Видалити з кошика";
                            var action = await Page.DisplayActionSheet("Дії:", "Закрити", null, changeQuantity, delete);
                            switch (action)
                            {
                                case changeQuantity:
                                    var result = await Page.DisplayPromptAsync(changeQuantity, "Введіть кількість",
                                        initialValue: x.Count.ToString(CultureInfo.InvariantCulture), keyboard: Keyboard.Numeric);
                                    x.Count = double.Parse(result);
                                    await ChangeCartItem(x);
                                    break;
                                case delete:
                                    await DeleteCartItem(x);
                                    break;
                            }
                        })
                    };
                })
            };
        
        query.ForEach(x => { ImageButtons.Add(new ImageButtonsGroup(x.Key, x.Result.ToList())); });

        HeaderText = "Доступно " + itemsToBuyListUngrouped.Count + " з " + itemsToBuyListPreProcessed.Count;

        Loaded?.Invoke(this,
            new LoadingArgs() { Success = true, LoadedCount = items.Count, Total = itemsToBuyListPreProcessed.Count });
    }

    private async Task ChangeCartItem(ItemToBuy model)
    {
        await _itemsToBuyLocalRepository.UpdateAsync(_mapper.Map<ItemToBuyLocalDatabaseModel>(model));
        await LoadAsync();
    }

    private async Task DeleteCartItem(ItemToBuy model)
    {
        await _itemsToBuyLocalRepository.DeleteAsync(model.RecordId);
        await LoadAsync();
    }

    public async Task ClearShoppingList()
    {
        ImageButtons = new ObservableCollection<ImageButtonsGroup>();
        HeaderText = "Доступно 0 з 0";
        await _itemsToBuyLocalRepository.DeleteAllAsync();
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
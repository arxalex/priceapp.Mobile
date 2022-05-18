using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using priceapp.Annotations;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(CartViewModel))]

namespace priceapp.ViewModels;

public class CartViewModel : ICartViewModel
{
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

    public ObservableCollection<ItemToBuy> ItemsList
    {
        get => _itemsList;
        set
        {
            _itemsList = value;
            OnPropertyChanged();
        }
    }

    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository;
    private readonly IShopRepository _shopRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IMapper _mapper;

    private bool _isRefreshing;
    private ObservableCollection<ItemToBuy> _itemsList = new();
    private const int RefreshDuration = 0;

    public CartViewModel()
    {
        _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();
        _shopRepository = DependencyService.Get<IShopRepository>();
        _itemRepository = DependencyService.Get<IItemRepository>();
        _mapper = DependencyService.Get<IMapper>();

        _itemsToBuyLocalRepository.BadConnectEvent += ItemsToBuyLocalRepositoryOnBadConnectEvent;
    }

    private void ItemsToBuyLocalRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    public async Task LoadAsync()
    {
        ItemsList = new ObservableCollection<ItemToBuy>();
        var items = await _itemsToBuyLocalRepository.GetItems();
        var filials = await _shopRepository.GetFilials();
        var shops = await _shopRepository.GetShops();

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

            ItemsList.Add(itemToBuy);
        }

        Loaded?.Invoke(this,
            new LoadingArgs() {Success = true, LoadedCount = items.Count, Total = ItemsList.Count});
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

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
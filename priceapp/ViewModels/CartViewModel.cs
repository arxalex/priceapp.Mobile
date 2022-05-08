using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AutoMapper;
using priceapp.Annotations;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Models;
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
        ItemsList.Clear();
        await Task.Delay(TimeSpan.FromSeconds(RefreshDuration));
        await LoadAsync();
        IsRefreshing = false;
    });

    public ObservableCollection<ItemToBuy> ItemsList { get; set; } = new();

    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository;
    private readonly IMapper _mapper;

    private bool _isRefreshing;
    private const int RefreshDuration = 2;

    public CartViewModel()
    {
        _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();
        _mapper = DependencyService.Get<IMapper>();

        _itemsToBuyLocalRepository.BadConnectEvent += ItemsToBuyLocalRepositoryOnBadConnectEvent;
    }

    private void ItemsToBuyLocalRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    public async Task LoadAsync()
    {
        var items = await _itemsToBuyLocalRepository.GetItems();

        foreach (var item in items)
        {
            ItemsList.Add(_mapper.Map<ItemToBuy>(item));
        }

        Loaded?.Invoke(this,
            new LoadingArgs() {Success = true, LoadedCount = items.Count, Total = ItemsList.Count});
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
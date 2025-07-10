using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class ItemsListPage
{
    private readonly IItemsListViewModel _itemsListViewModel;
    private bool _isBusy;

    public ItemsListPage(int categoryId, string? categoryName, IItemsListViewModel itemsListViewModel)
    {
        InitializeComponent();
        CategoryName = categoryName;
        _itemsListViewModel = itemsListViewModel;
        IsBusy = false;
        HeaderBackButton.Label = CategoryName;

        _itemsListViewModel.CategoryId = categoryId;
        _itemsListViewModel.Loaded += ItemsListViewModelOnLoaded;
        _itemsListViewModel.BadConnectEvent += ItemsListViewModelOnBadConnectEvent;

        CollectionGrid.RemainingItemsThreshold = 2;
        CollectionGrid.RemainingItemsThresholdReached += CollectionViewOnRemainingThresholdReached;

        BindingContext = _itemsListViewModel;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        NotFound.IsVisible = false;
        CollectionGrid.IsVisible = false;
        _itemsListViewModel.LoadAsync(Navigation);
    }

    private string? CategoryName { get; set; }

    private void ItemsListViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void ItemsListViewModelOnLoaded(object sender, LoadingArgs args)
    {
        _isBusy = false;
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        CollectionGrid.IsVisible = true;
        NotFound.IsVisible = false;

        if (!args.Success)
        {
            CollectionGrid.RemainingItemsThreshold = -1;
        }

        if (args.Total < 1)
        {
            CollectionGrid.IsVisible = false;
            NotFound.IsVisible = true;
        }
    }

    private void CollectionViewOnRemainingThresholdReached(object sender, EventArgs e)
    {
        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        _itemsListViewModel.LoadAsync(Navigation);
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
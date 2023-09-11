using System;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ItemsListPage
{
    private readonly IItemsListViewModel _itemsListViewModel = DependencyService.Get<IItemsListViewModel>(DependencyFetchTarget.NewInstance);
    private bool _isBusy;

    public ItemsListPage(int categoryId, string categoryName)
    {
        InitializeComponent();
        CategoryName = categoryName;
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
        CollectionGrid.IsVisible = false;
        _itemsListViewModel.LoadAsync(Navigation);
    }

    private string CategoryName { get; set; }

    private async void ItemsListViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        await Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void ItemsListViewModelOnLoaded(object sender, LoadingArgs args)
    {
        _isBusy = false;
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        CollectionGrid.IsVisible = true;
        if (!args.Success)
        {
            CollectionGrid.RemainingItemsThreshold = -1;
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
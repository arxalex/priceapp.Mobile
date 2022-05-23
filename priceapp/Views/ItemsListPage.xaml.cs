using System;
using System.Linq;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ItemsListPage
{
    private readonly IItemsListViewModel _itemsListViewModel;
    private bool isBusy;

    public ItemsListPage(int categoryId, string categoryName)
    {
        InitializeComponent();
        CategoryName = categoryName;
        IsBusy = false;
        CategoryLabel.Text = CategoryName;

        _itemsListViewModel = DependencyService.Get<IItemsListViewModel>(DependencyFetchTarget.NewInstance);
        _itemsListViewModel.CategoryId = categoryId;
        _itemsListViewModel.Loaded += ItemsListViewModelOnLoaded;
        _itemsListViewModel.BadConnectEvent += ItemsListViewModelOnBadConnectEvent;

        CollectionView.RemainingItemsThreshold = 2;
        CollectionView.RemainingItemsThresholdReached += CollectionViewOnRemainingThresholdReached;

        BindingContext = _itemsListViewModel;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        CollectionView.IsVisible = false;
        _itemsListViewModel.LoadAsync();
    }

    private string CategoryName { get; set; }

    private async void ItemsListViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        await Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void ItemsListViewModelOnLoaded(object sender, LoadingArgs args)
    {
        isBusy = false;
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        CollectionView.IsVisible = true;
        if (!args.Success)
        {
            CollectionView.RemainingItemsThreshold = -1;
        }
    }

    private void CollectionViewOnRemainingThresholdReached(object sender, EventArgs e)
    {
        if (isBusy)
        {
            return;
        }

        isBusy = true;
        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        _itemsListViewModel.LoadAsync();
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void CollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CollectionView.SelectedItem == null) return;
        var item = (Item) e.CurrentSelection.FirstOrDefault()!;
        await Navigation.PushAsync(new ItemPage(item));
        CollectionView.SelectedItem = null;
    }
}
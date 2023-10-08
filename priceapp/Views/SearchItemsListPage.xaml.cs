using System;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SearchItemsListPage
{
    private readonly ISearchItemsListViewModel _searchItemsListViewModel = DependencyService.Get<ISearchItemsListViewModel>(DependencyFetchTarget.NewInstance);
    private bool _isBusy;

    public SearchItemsListPage(string search)
    {
        InitializeComponent();
        IsBusy = false;
        HeaderBackButton.Label = search;

        _searchItemsListViewModel.Loaded += SearchItemsListViewModelOnLoaded;
        _searchItemsListViewModel.BadConnectEvent += SearchItemsListViewModelOnBadConnectEvent;
        _searchItemsListViewModel.Search = search;

        CollectionView.RemainingItemsThreshold = 2;
        CollectionView.RemainingItemsThresholdReached += CollectionViewOnRemainingThresholdReached;

        BindingContext = _searchItemsListViewModel;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        CollectionView.IsVisible = false;
        _searchItemsListViewModel.LoadAsync(Navigation);
    }

    private void SearchItemsListViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void SearchItemsListViewModelOnLoaded(object sender, LoadingArgs args)
    {
        _isBusy = false;
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
        if (_isBusy)
        {
            return;
        }

        _isBusy = true;
        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        _searchItemsListViewModel.LoadAsync(Navigation);
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}
using System;
using System.Linq;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SearchItemsListPage
{
    private readonly ISearchItemsListViewModel _searchItemsListViewModel;
    private bool _isBusy;

    public SearchItemsListPage(string search)
    {
        InitializeComponent();
        IsBusy = false;
        CategoryLabel.Text = search;

        _searchItemsListViewModel = DependencyService.Get<ISearchItemsListViewModel>(DependencyFetchTarget.NewInstance);
        _searchItemsListViewModel.Loaded += SearchItemsListViewModelOnLoaded;
        _searchItemsListViewModel.BadConnectEvent += SearchItemsListViewModelOnBadConnectEvent;
        _searchItemsListViewModel.Search = search;

        CollectionView.RemainingItemsThreshold = 2;
        CollectionView.RemainingItemsThresholdReached += CollectionViewOnRemainingThresholdReached;

        BindingContext = _searchItemsListViewModel;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        CollectionView.IsVisible = false;
        _searchItemsListViewModel.LoadAsync();
    }

    private async void SearchItemsListViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        await Navigation.PushAsync(new ConnectionErrorPage(args));
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
        _searchItemsListViewModel.LoadAsync();
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
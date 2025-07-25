using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class SearchPage
{
    private readonly ISearchViewModel _searchViewModel;
    private readonly IServiceProvider _serviceProvider;

    public SearchPage(ISearchViewModel searchViewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _searchViewModel = searchViewModel;
        _serviceProvider = serviceProvider;

        _searchViewModel.Loaded += SearchViewModelOnLoaded;
        _searchViewModel.BadConnectEvent += SearchViewModelOnBadConnectEvent;

        BindingContext = _searchViewModel;

        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        CollectionView.IsVisible = false;
        NotFound.IsVisible = false;
    }

    private void SearchViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void SearchViewModelOnLoaded(object sender, LoadingArgs args)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        CollectionView.IsVisible = true;
        NotFound.IsVisible = false;

        if (args.Total < 1)
        {
            NotFound.IsVisible = true;
            CollectionView.IsVisible = false;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Device.BeginInvokeOnMainThread(Action);
        return;

        async void Action()
        {
            await Task.Delay(250);
            SearchEntry.Focus();
        }
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void SearchEntry_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (SearchEntry.Text.Length < 3) return;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        CollectionView.IsVisible = false;
        _searchViewModel.Search = SearchEntry.Text;
        await _searchViewModel.LoadAsync(Navigation);
    }

    private void SearchEntry_OnCompleted(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SearchItemsListPage(SearchEntry.Text, _serviceProvider.GetRequiredService<ISearchItemsListViewModel>()));
    }
}
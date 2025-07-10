using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class CatalogAndSearchPage
{
    private readonly IServiceProvider _serviceProvider;
    public CatalogAndSearchPage(ICategoryViewModel categoryViewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        categoryViewModel.Loaded += CategoryViewModelOnLoaded;
        categoryViewModel.BadConnectEvent += CategoryViewModelOnBadConnectEvent;
            
        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        CollectionGrid.IsVisible = false;
        categoryViewModel.LoadAsync(Navigation);

        BindingContext = categoryViewModel;
    }

    private void CategoryViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void CategoryViewModelOnLoaded(object sender, LoadingArgs args)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        CollectionGrid.IsVisible = true;
    }

    private void SearchButton_OnTapped(object sender, EventArgs e)
    {
        Navigation.PushAsync(new SearchPage(_serviceProvider.GetRequiredService<ISearchViewModel>(), _serviceProvider));
    }
}
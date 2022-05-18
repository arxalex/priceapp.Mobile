using System.Collections.Generic;
using priceapp.Enums;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class CartProcessedPage
{
    private readonly ICartProcessedViewModel _cartProcessedViewModel;
    public CartProcessedPage(IList<ItemToBuy> items, CartProcessingType type)
    {
        InitializeComponent();
        
        _cartProcessedViewModel = DependencyService.Get<ICartProcessedViewModel>(DependencyFetchTarget.NewInstance);
            
        _cartProcessedViewModel.Loaded += CartProcessedViewModelOnLoaded;
        _cartProcessedViewModel.BadConnectEvent += CartProcessedViewModelOnBadConnectEvent;
            
        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        CollectionView.IsVisible = false;
        _cartProcessedViewModel.LoadAsync(items, type);

        BindingContext = _cartProcessedViewModel;
    }

    private async void CartProcessedViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        await Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void CartProcessedViewModelOnLoaded(object sender, LoadingArgs args)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        CollectionView.IsVisible = true;
    }
}
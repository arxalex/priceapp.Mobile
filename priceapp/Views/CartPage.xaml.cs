using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class CartPage
{
    private readonly ICartViewModel _cartViewModel;

    public CartPage(ICartViewModel cartViewModel)
    {
        InitializeComponent();
        _cartViewModel = cartViewModel;

        _cartViewModel.Loaded += CartViewModelOnLoaded;
        _cartViewModel.BadConnectEvent += CartViewModelOnBadConnectEvent;
        _cartViewModel.Page = this;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        _cartViewModel.LoadAsync();

        BindingContext = _cartViewModel;
    }

    private void CartViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private async void CartViewModelOnLoaded(object sender, LoadingArgs args)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        if (args.Success == false)
        {
            await DisplayAlert("Обрані товари відсутні в одному магазині",
                "Спробуйте змінити спосіб формування списку покупок або збільшити радіус пошуку у налаштуваннях",
                "Ок");
        }
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        await _cartViewModel.ClearShoppingList();
    }
}
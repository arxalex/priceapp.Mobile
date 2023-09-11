using System;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage
    {
        private readonly ICartViewModel _cartViewModel = DependencyService.Get<ICartViewModel>(DependencyFetchTarget.NewInstance);

        public CartPage()
        {
            InitializeComponent();

            _cartViewModel.Loaded += CartViewModelOnLoaded;
            _cartViewModel.BadConnectEvent += CartViewModelOnBadConnectEvent;
            _cartViewModel.Page = this;

            ActivityIndicator.IsRunning = true;
            ActivityIndicator.IsVisible = true;
            _cartViewModel.LoadAsync();

            BindingContext = _cartViewModel;
        }

        private async void CartViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
        {
            await Navigation.PushAsync(new ConnectionErrorPage(args));
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
}
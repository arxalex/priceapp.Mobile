using System;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage
    {
        private readonly ICartViewModel _cartViewModel;
        public CartPage()
        {
            InitializeComponent();
            
            _cartViewModel = DependencyService.Get<ICartViewModel>(DependencyFetchTarget.NewInstance);
            
            _cartViewModel.Loaded += CartViewModelOnLoaded;
            _cartViewModel.BadConnectEvent += CartViewModelOnBadConnectEvent;
            
            ActivityIndicator.IsRunning = true;
            ActivityIndicator.IsVisible = true;
            CollectionView.IsVisible = false;
            _cartViewModel.LoadAsync();

            BindingContext = _cartViewModel;
        }

        private async void CartViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
        {
            await Navigation.PushAsync(new ConnectionErrorPage(args));
        }

        private void CartViewModelOnLoaded(object sender, LoadingArgs args)
        {
            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
            CollectionView.IsVisible = true;
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            const string oneMarket = "Все в одному магазині";
            const string oneMarketLowest = "Найдешевший варіант в одному магазині, окрім тих товарів, що вже вибрані";
            const string manyMarketsLowest = "Найдешевший варіант в декількох магазинах";

            var action = await DisplayActionSheet("Метод формування списку:", "Закрити", null, oneMarket, oneMarketLowest, manyMarketsLowest);
            switch (action)
            {
                case oneMarket:
                    break;
                case oneMarketLowest:
                    break;
                case manyMarketsLowest:
                    break;
            }
        }
        
        private async void CollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CollectionView.SelectedItem == null) return;
            const string changeQuantity = "Змінити кількість";
            const string delete = "Видалити з кошика";
            var action = await DisplayActionSheet("Дії:", "Закрити", null, changeQuantity, delete);
            var item = (ItemToBuy) CollectionView.SelectedItem;
            switch (action)
            {
                case changeQuantity:
                    var result = await DisplayPromptAsync(changeQuantity, "Введіть кількість", initialValue: item.Count.ToString(), keyboard: Keyboard.Numeric);
                    item.Count = double.Parse(result);
                    await _cartViewModel.ChangeCartItem(item);
                    break;
                case delete:
                    await _cartViewModel.DeleteCartItem(item);
                    break;
            }

            CollectionView.SelectedItem = null;
        }
    }
}
using priceapp.Events.Models;
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
    }
}
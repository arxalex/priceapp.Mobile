using System;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogAndSearchPage
    {
        public CatalogAndSearchPage()
        {
            InitializeComponent();
            var categoryViewModel = DependencyService.Get<ICategoryViewModel>(DependencyFetchTarget.NewInstance);
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
            Navigation.PushAsync(new SearchPage());
        }
    }
}
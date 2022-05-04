using System;
using System.Linq;
using priceapp.Events.Models;
using priceapp.Models;
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
            CollectionView.IsVisible = false;
            categoryViewModel.LoadAsync();

            BindingContext = categoryViewModel;
        }

        private async void CategoryViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
        {
            await Navigation.PushAsync(new ConnectionErrorPage(args));
        }

        private void CategoryViewModelOnLoaded(object sender, LoadingArgs args)
        {
            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
            CollectionView.IsVisible = true;
        }

        private async void SearchButton_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage());
        }

        private async void SelectableItemsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CollectionView.SelectedItem == null) return;
            var category = (Category) e.CurrentSelection.FirstOrDefault()!;
            await Navigation.PushAsync(new ItemsListPage(category.Id, category.Label));
            CollectionView.SelectedItem = null;
        }
    }
}
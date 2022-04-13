using System;
using System.Linq;
using priceapp.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogAndSearchPage : ContentPage
    {
        public ICategoryViewModel _categoryViewModel;

        public CatalogAndSearchPage()
        {
            InitializeComponent();
            _categoryViewModel = DependencyService.Get<ICategoryViewModel>();
            _categoryViewModel.Load();

            BindingContext = _categoryViewModel;
        }

        private void SearchButton_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SearchPage());
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
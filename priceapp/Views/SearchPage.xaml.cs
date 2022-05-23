using System;
using System.Linq;
using System.Threading.Tasks;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage
    {
        private readonly ISearchViewModel _searchViewModel;

        public SearchPage()
        {
            InitializeComponent();

            _searchViewModel = DependencyService.Get<ISearchViewModel>(DependencyFetchTarget.NewInstance);

            _searchViewModel.Loaded += SearchViewModelOnLoaded;
            _searchViewModel.BadConnectEvent += SearchViewModelOnBadConnectEvent;

            BindingContext = _searchViewModel;

            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
            CollectionView.IsVisible = false;
        }

        private async void SearchViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
        {
            await Navigation.PushAsync(new ConnectionErrorPage(args));
        }

        private void SearchViewModelOnLoaded(object sender, LoadingArgs args)
        {
            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
            CollectionView.IsVisible = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Task.Delay(250);
                SearchEntry.Focus();
            });
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
            await _searchViewModel.LoadAsync();
        }

        private async void SearchEntry_OnCompleted(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SearchItemsListPage(SearchEntry.Text));
        }

        private async void CollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CollectionView.SelectedItem == null) return;
            var item = (Item) e.CurrentSelection.FirstOrDefault()!;
            await Navigation.PushAsync(new ItemPage(item));
            CollectionView.SelectedItem = null;
        }
    }
}
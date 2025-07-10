using System.ComponentModel;
using Microsoft.Maui.Maps;
using priceapp.Events.Models;
using priceapp.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class ItemPage
{
    private readonly IItemViewModel _itemViewModel;

    public ItemPage(
        Item? item, 
        IItemViewModel itemViewModel,
        ILocationService locationService
        ) {
        InitializeComponent();
        _itemViewModel = itemViewModel;
        
        _itemViewModel.Loaded += ItemViewModelOnLoaded;
        _itemViewModel.BadConnectEvent += ItemViewModelOnBadConnectEvent;
        _itemViewModel.PropertyChanged += ItemViewModelOnPropertyChanged;
        var currentPosition = locationService.GetLocationAsync().Result;
        Map.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                new Location(currentPosition.Latitude, currentPosition.Longitude),
                Distance.FromMeters(Preferences.Get("locationRadius", 1000) + 100)
            )
        );

        BindingContext = _itemViewModel;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;
        ItemInfo.IsVisible = false;
        _itemViewModel.LoadAsync(item, this);
    }

    private void ItemViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void ItemViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        var item = _itemViewModel.Item;
        UnitsLabel.IsVisible = UnitsLabelValue.IsVisible = item.UnitsText.Length > 0;
        BrandLabel.IsVisible = BrandLabelValue.IsVisible = item.BrandLabel.Length > 0;
        CategoryLabel.IsVisible = CategoryLabelValue.IsVisible = item.CategoryLabel.Length > 0;
        PackageLabel.IsVisible = PackageLabelValue.IsVisible = item.PackageLabel.Length > 0;
        CalorieLabel.IsVisible = CalorieLabelValue.IsVisible = item.Calorie > 0;
        FatLabel.IsVisible = FatLabelValue.IsVisible = item.Fat > 0;
        CarbohydratesLabel.IsVisible = CarbohydratesLabelValue.IsVisible = item.Carbohydrates > 0;
        ProteinsLabel.IsVisible = ProteinsLabelValue.IsVisible = item.Proteins > 0;
    }

    private void ItemViewModelOnLoaded(object sender, LoadingArgs args)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
        ItemInfo.IsVisible = true;
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        await _itemViewModel.AddToCart();
    }
}
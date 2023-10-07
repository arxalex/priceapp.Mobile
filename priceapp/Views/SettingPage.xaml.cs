using System;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingPage
{
    private readonly ISettingsViewModel _settingsViewModel = DependencyService.Get<ISettingsViewModel>(DependencyFetchTarget.NewInstance);

    public SettingPage()
    {
        InitializeComponent();

        var locationService = DependencyService.Get<ILocationService>();
        var currentPosition = locationService.GetLocationAsync().Result;
        Map.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                new Position(currentPosition.Latitude, currentPosition.Longitude),
                Distance.FromMeters(Preferences.Get("locationRadius", 1000) + 100)
            )
        );
        Map.MapClicked += OnMapClicked;
        
        BindingContext = _settingsViewModel;
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    
    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        var position = e.Position;
        _settingsViewModel.CustomLocation = new Location(position.Latitude, position.Longitude);
    }
}
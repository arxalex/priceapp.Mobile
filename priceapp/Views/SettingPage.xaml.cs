using System;
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
    private const string CustomLocationPinLabel = "Вибрана геопозиція";
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Map.MapClicked += OnMapClicked;
        var customLocation = _settingsViewModel.CustomLocation;
        Map.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                new Position(customLocation.Latitude, customLocation.Longitude),
                Distance.FromMeters(Preferences.Get("locationRadius", 1000) + 100)
            )
        );
        var selectedPin = new Pin
        {
            Type = PinType.SavedPin,
            Position = new Position(customLocation.Latitude, customLocation.Longitude),
            Label = CustomLocationPinLabel
        };
        Map.Pins.Add(selectedPin);
    }

    public SettingPage()
    {
        InitializeComponent();
        
        BindingContext = _settingsViewModel;
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    
    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        Map.Pins.Clear();

        var position = e.Position;

        var selectedPin = new Pin
        {
            Type = PinType.SavedPin,
            Position = position,
            Label = CustomLocationPinLabel
        };
        
        Map.Pins.Add(selectedPin);
        
        _settingsViewModel.CustomLocation = new Location(position.Latitude, position.Longitude);
    }
}
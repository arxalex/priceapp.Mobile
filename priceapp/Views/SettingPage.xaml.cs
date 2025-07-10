using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class SettingPage
{
    private readonly ISettingsViewModel _settingsViewModel;
    private const string CustomLocationPinLabel = "Вибрана геопозиція";
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Map.MapClicked += OnMapClicked;
        var customLocation = _settingsViewModel.CustomLocation;
        Map.MoveToRegion(
            MapSpan.FromCenterAndRadius(
                new Location(customLocation.Latitude, customLocation.Longitude),
                Distance.FromMeters(Preferences.Get("locationRadius", 1000) + 100)
            )
        );
        var selectedPin = new Pin
        {
            Type = PinType.SavedPin,
            Location = new Location(customLocation.Latitude, customLocation.Longitude),
            Label = CustomLocationPinLabel
        };
        Map.Pins.Add(selectedPin);
    }

    public SettingPage(ISettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        _settingsViewModel = settingsViewModel;
        
        BindingContext = _settingsViewModel;
    }

    private async void ImageButton_OnClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
    
    private void OnMapClicked(object sender, MapClickedEventArgs e)
    {
        Map.Pins.Clear();

        var position = e.Location;

        var selectedPin = new Pin
        {
            Type = PinType.SavedPin,
            Location = position,
            Label = CustomLocationPinLabel
        };
        
        Map.Pins.Add(selectedPin);
        
        _settingsViewModel.CustomLocation = new Location(position.Latitude, position.Longitude);
    }
}
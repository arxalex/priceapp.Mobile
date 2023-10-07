using System;
using System.Threading.Tasks;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using priceapp.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(LocationService))]

namespace priceapp.Services.Implementation;

public class LocationService : ILocationService
{
    public bool UseCustomLocation
    {
        get => Preferences.Get("useCustomLocation", Constants.DefaultUseCustomLocation);
        set
        {
            Preferences.Set("useCustomLocation", value);
            RefreshLocation().Wait();
        }
    }

    /*
     * For setup custom location only. Use GetLocationAsync() to properly get location
     */
    public Location CustomLocation
    {
        get
        {
            var cord = CustomLocationString.Split(';');

            var x = double.Parse(cord[0]);
            var y = double.Parse(cord[1]);

            return new Location(y, x);
        }
        set => CustomLocationString = $"{value.Longitude};{value.Latitude}";
    }
    
    private string CustomLocationString
    {
        get => Preferences.Get("customLocation", Constants.DefaultCustomLocation);
        set => Preferences.Set("customLocation", value);
    }

    public async Task<Location> GetLocationAsync()
    {
        try
        {
            if (UseCustomLocation)
            {
                return CustomLocation;
            }
            
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(3));
            
            return await Geolocation.GetLastKnownLocationAsync() ?? await Geolocation.GetLocationAsync(request);
        }
        catch (Exception)
        {
            await TrySetCustomLocation();

            return new Location(30, 50);
        }
    }

    public async Task RefreshLocation()
    {
        if (!UseCustomLocation)
        {
            try
            {
                await RefreshLocationRequest();
            }
            catch (Exception)
            {
                await RefreshPermission();
            }
        }
    }

    public async Task RefreshPermission()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                await TrySetCustomLocation();
            }
            else
            {
                await RefreshLocationRequest();
            } 
        }
    }

    public void ClearCustomLocationData()
    {
        CustomLocationString = Constants.DefaultCustomLocation;
        UseCustomLocation = Constants.DefaultUseCustomLocation;
    }

    private async Task TrySetCustomLocation()
    {
        if (Application.Current.MainPage.GetType() == typeof(MainPage))
        {
            const string addAction = "Перейти до налаштувань";
            var action = await Application.Current.MainPage.DisplayActionSheet(
                "Неможливо отримати вашу геопозицію. Вкажіть вашу геопозицію в налаштуваннях", "Закрити", null,
                addAction);
            if (action == addAction)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new SettingPage());
            }
        }
            
        await Application.Current.MainPage.DisplayAlert("Неможливо отримати вашу геопозицію", "Вкажіть вашу геопозицію в налаштуваннях", "Закрити");
    }

    private async Task RefreshLocationRequest()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(3));
        await Geolocation.GetLocationAsync(request);
    }
}
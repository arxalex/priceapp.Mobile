using System;
using System.Threading.Tasks;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

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
            RefreshLocation();
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
        catch (Exception e)
        {
            DisplayAlert(e);

            return new Location(Constants.DefaultY, Constants.DefaultX);
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
                DisplayAlert(new PermissionException(""));
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

    private static async Task DisplayAlert(Exception e)
    {
        var message = "Вкажіть вашу геопозицію в налаштуваннях";
        if (e != null)
        {
            if (e.GetType() == typeof(PermissionException))
            {
                message = "Надайте додатку доступ до геолокації в налаштуваннях пристрою";
            }
            else if (e.GetType() == typeof(FeatureNotEnabledException))
            {
                message = "Увімкніть GPS";
            }
        }

        await Application.Current.MainPage.DisplayAlert("Неможливо отримати вашу геопозицію", message, "Закрити");
    }

    private static async Task RefreshLocationRequest()
    {
        var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(3));
        await Geolocation.GetLocationAsync(request);
    }
}
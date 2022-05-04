using System;
using System.Threading.Tasks;
using priceapp.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(GeolocationUtil))]
namespace priceapp.Utils;

public class GeolocationUtil
{
    public async Task<Location> GetCurrentLocation()
    {
        try
        {
            return await Geolocation.GetLastKnownLocationAsync() ?? new Location(30, 50);
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            return new Location(30, 50);
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
            return new Location(30, 50);
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            return new Location(30, 50);
        }
        catch (Exception ex)
        {
            // Unable to get location
            return new Location(30, 50);
        }
    }

    public async Task<Location> GetCurrentLocationNow()
    {
        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(3));
            
            return await Geolocation.GetLocationAsync(request) ?? new Location(30, 50);
        }
        catch (FeatureNotSupportedException fnsEx)
        {
            // Handle not supported on device exception
            return new Location(30, 50);
        }
        catch (FeatureNotEnabledException fneEx)
        {
            // Handle not enabled on device exception
            return new Location(30, 50);
        }
        catch (PermissionException pEx)
        {
            // Handle permission exception
            return new Location(30, 50);
        }
        catch (Exception ex)
        {
            // Unable to get location
            return new Location(30, 50);
        }
    }
}
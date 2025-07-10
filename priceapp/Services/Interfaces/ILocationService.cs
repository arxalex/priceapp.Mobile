namespace priceapp.Services.Interfaces;

public interface ILocationService
{
    bool UseCustomLocation { get; set; }
    Location CustomLocation { get; set; }
    Task<Location> GetLocationAsync();
    Task RefreshPermission();
    Task RefreshLocation();
    void ClearCustomLocationData();
}
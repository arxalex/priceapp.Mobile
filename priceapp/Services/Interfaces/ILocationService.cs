namespace priceapp.Services.Interfaces;

public interface ILocationService
{
    bool UseCustomLocation { get; set; }
    Location CustomLocation { get; set; }
    Task<Location> GetLocationAsync();
    void ClearCustomLocationData();
}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using priceapp.Annotations;
using priceapp.Enums;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.ViewModels;

public class SettingsViewModel : ISettingsViewModel
{
    private readonly ILocationService _locationService;

    public SettingsViewModel(ILocationService locationService)
    {
        _locationService = locationService;
    }

    private Dictionary<CartProcessingType, string> CartProcessingTypesDictionary { get; set; } =
        new()
        {
            {CartProcessingType.MultipleMarketsLowest, "В різних магазинах"},
            {CartProcessingType.OneMarketLowest, "В одному магазині"}
        };

    public int Radius
    {
        get => Preferences.Get("locationRadius", Constants.DefaultRadius);
        set
        {
            Preferences.Set("locationRadius", value);
            OnPropertyChanged();
        }
    }

    public bool ShowRussiaSupportBrandAlerts
    {
        get => Preferences.Get("showRussiaSupportBrandAlerts",
            Constants.DefaultShowRussiaSupportBrandAlerts);
        set
        {
            Preferences.Set("showRussiaSupportBrandAlerts", value);
            OnPropertyChanged();
        }
    }
    
    public bool UseCustomLocation
    {
        get => _locationService.UseCustomLocation;
        set
        {
            _locationService.UseCustomLocation = value;
            OnPropertyChanged();
        }
    }
    
    public Location CustomLocation
    {
        get => _locationService.CustomLocation;
        set
        {
            _locationService.CustomLocation = value;
            OnPropertyChanged();
        }
    }

    public string CartProcessingTypeSetting
    {
        get
        {
            var setting = (CartProcessingType) Preferences.Get("cartProcessingType",
                (int) CartProcessingType.MultipleMarketsLowest);
            return CartProcessingTypesDictionary[setting];
        }
        set
        {
            var setting = CartProcessingTypesDictionary.First(x => x.Value == value);
            Preferences.Set("cartProcessingType", (int) setting.Key);
            OnPropertyChanged();
        }
    }

    public List<string> CartProcessingTypes => CartProcessingTypesDictionary.Values.ToList();

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
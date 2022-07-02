using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using priceapp.Annotations;
using priceapp.Enums;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;

[assembly: Xamarin.Forms.Dependency(typeof(SettingsViewModel))]

namespace priceapp.ViewModels;

public class SettingsViewModel : ISettingsViewModel
{
    private Dictionary<CartProcessingType, string> CartProcessingTypesDictionary { get; set; } =
        new()
        {
            {CartProcessingType.MultipleMarketsLowest, "В різних магазинах"},
            {CartProcessingType.OneMarketLowest, "В одному магазині"}
        };

    public int Radius
    {
        get => Xamarin.Essentials.Preferences.Get("locationRadius", Constants.DefaultRadius);
        set
        {
            Xamarin.Essentials.Preferences.Set("locationRadius", value);
            OnPropertyChanged();
        }
    }

    public string CartProcessingTypeSetting
    {
        get
        {
            var setting = (CartProcessingType) Xamarin.Essentials.Preferences.Get("cartProcessingType",
                (int) CartProcessingType.MultipleMarketsLowest);
            return CartProcessingTypesDictionary[setting];
        }
        set
        {
            var setting = CartProcessingTypesDictionary.First(x => x.Value == value);
            Xamarin.Essentials.Preferences.Set("cartProcessingType", (int) setting.Key);
            OnPropertyChanged();
        }
    }

    public List<string> CartProcessingTypes => CartProcessingTypesDictionary.Values.ToList();

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
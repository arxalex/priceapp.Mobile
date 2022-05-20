using System.Collections.Generic;
using System.ComponentModel;

namespace priceapp.ViewModels.Interfaces;

public interface ISettingsViewModel : INotifyPropertyChanged
{
    int Radius { get; set; }
    List<string> CartProcessingTypes { get; }
    string CartProcessingTypeSetting { get; set; }
}
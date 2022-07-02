using System.Collections.ObjectModel;
using System.ComponentModel;
using priceapp.UI;

namespace priceapp.ViewModels.Interfaces;

public interface IOnboardingViewModel : INotifyPropertyChanged
{
    ObservableCollection<OnboardingItem> Items { get; set; }
    string NextButtonText { get; set; }
    string SkipButtonText { get; set; }
    int Position { get; set; }
}
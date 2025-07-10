using System.ComponentModel;
using priceapp.Events.Delegates;

namespace priceapp.ViewModels.Interfaces;

public interface IDeleteAccountViewModel : INotifyPropertyChanged
{
    event DeleteAccountHandler DeleteSuccess;
    string? Username { get; set; }
    string? Email { get; set; }
    Task DeleteUser(string password);
    event LoadingHandler Loaded;
    Task LoadAsync();
}
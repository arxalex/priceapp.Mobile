using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.UI;

namespace priceapp.ViewModels.Interfaces;

public interface IAccountViewModel : INotifyPropertyChanged
{
    ObservableCollection<MenuItem> MenuItems { get; set; }
    string Username { get; set; }
    string Email { get; set; }
    event ConnectionErrorHandler BadConnectEvent;
    Task LoadAsync();
    event LoadingHandler Loaded;
    Task ChangeAccount();
}
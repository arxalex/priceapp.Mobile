using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using priceapp.Events.Delegates;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface ICartViewModel : INotifyPropertyChanged
{
    Task LoadAsync();
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
    ObservableCollection<ItemToBuy> ItemsList { get; set; }
    bool IsRefreshing { get; set; }
    ICommand RefreshCommand { get; }
}
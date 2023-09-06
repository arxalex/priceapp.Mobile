using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Models;
using Xamarin.Forms;

namespace priceapp.ViewModels.Interfaces;

public interface ICartViewModel : INotifyPropertyChanged
{
    bool IsRefreshing { get; set; }
    ICommand RefreshCommand { get; }
    ObservableCollection<ImageButtonsGroup> ImageButtons { get; set; }
    string HeaderText { get; set; }
    string Economy { get; set; }
    Page Page { get; set; }
    Task LoadAsync();
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
    Task ClearShoppingList();
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using priceapp.Events.Delegates;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface ICartViewModel : INotifyPropertyChanged
{
    List<ItemToBuy> ItemsToBuyListPreProcessed { get; set; }
    bool IsRefreshing { get; set; }
    ICommand RefreshCommand { get; }
    ObservableCollection<ItemToBuyGroup> ItemsToBuyList { get; set; }
    string HeaderText { get; set; }
    string Economy { get; set; }
    Task LoadAsync();
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
    Task ChangeCartItem(ItemToBuy model);
    Task DeleteCartItem(ItemToBuy model);
    Task ClearShoppingList();
}
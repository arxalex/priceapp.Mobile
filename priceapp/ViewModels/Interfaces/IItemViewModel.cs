using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface IItemViewModel : INotifyPropertyChanged
{
    Item Item { get; set; }
    ObservableCollection<ItemPriceInfo> PricesAndFilials { get; set; }
    Task LoadAsync(Item item);
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface IItemsListViewModel
{
    ObservableCollection<Item> Items { get; set; }
    int CategoryId { get; set; }
    Task LoadAsync();
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
}
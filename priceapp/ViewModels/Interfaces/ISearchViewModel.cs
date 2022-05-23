using System.Collections.ObjectModel;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface ISearchViewModel
{
    ObservableCollection<Item> Items { get; set; }
    string Search { get; set; }
    Task LoadAsync();
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
}
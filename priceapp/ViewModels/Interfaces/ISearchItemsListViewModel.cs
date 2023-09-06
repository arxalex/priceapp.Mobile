using System.Collections.ObjectModel;
using System.Threading.Tasks;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using Xamarin.Forms;

namespace priceapp.ViewModels.Interfaces;

public interface ISearchItemsListViewModel
{
    string Search { get; set; }
    ObservableCollection<ImageButtonModel> ItemButtons { get; set; }
    Task LoadAsync(INavigation navigation);
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
}
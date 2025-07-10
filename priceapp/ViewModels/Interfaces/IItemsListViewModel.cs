using System.Collections.ObjectModel;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;


namespace priceapp.ViewModels.Interfaces;

public interface IItemsListViewModel
{
    int CategoryId { get; set; }
    ObservableCollection<ImageButtonModel> ItemButtons { get; set; }
    Task LoadAsync(INavigation navigation);
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
}
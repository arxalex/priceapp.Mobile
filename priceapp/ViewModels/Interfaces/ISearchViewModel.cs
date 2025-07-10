using System.Collections.ObjectModel;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;


namespace priceapp.ViewModels.Interfaces;

public interface ISearchViewModel
{
    string? Search { get; set; }
    ObservableCollection<ImageButtonModel> ItemButtons { get; set; }
    Task LoadAsync(INavigation navigation);
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
}
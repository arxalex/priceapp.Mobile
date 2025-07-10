using System.Collections.ObjectModel;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;


namespace priceapp.ViewModels.Interfaces;

public interface ICategoryViewModel
{
    ObservableCollection<ImageButtonModel> CategoryButtons { get; set; }
    Task LoadAsync(INavigation Navigation);
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
}
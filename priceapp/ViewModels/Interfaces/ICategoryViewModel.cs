using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Models;
using Xamarin.Forms;

namespace priceapp.ViewModels.Interfaces
{
    public interface ICategoryViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
        ObservableCollection<ImageButtonModel> CategoryButtons { get; set; }
        Task LoadAsync(INavigation Navigation);
        event LoadingHandler Loaded;
        event ConnectionErrorHandler BadConnectEvent;
    }
}
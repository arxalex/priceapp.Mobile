using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces
{
    public interface ICategoryViewModel
    {
        ObservableCollection<Category> Categories { get; set; }
        Task LoadAsync();
        event LoadingHandler Loaded;
        event ConnectionErrorHandler BadConnectEvent;
    }
}
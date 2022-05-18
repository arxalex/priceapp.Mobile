using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using priceapp.Enums;
using priceapp.Events.Delegates;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface ICartProcessedViewModel
{
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
    ObservableCollection<ItemToBuy> ItemsList { get; set; }
    Task LoadAsync(IList<ItemToBuy> items, CartProcessingType type);
}
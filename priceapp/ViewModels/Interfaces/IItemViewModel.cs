using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using priceapp.Controls.Models;
using priceapp.Events.Delegates;
using priceapp.Models;
using Xamarin.Forms;

namespace priceapp.ViewModels.Interfaces;

public interface IItemViewModel : INotifyPropertyChanged
{
    Item Item { get; set; }
    ObservableCollection<ItemPriceInfo> PricesAndFilials { get; set; }
    ObservableCollection<ImageButtonModel> ItemButtons { get; set; }
    BrandAlert BrandAlert { get; set; }
    bool IsVisibleBrandAlert { get; }
    Color ForeGroundColorBrandAlert { get; set; }
    Task LoadAsync(Item item, Page page);
    event LoadingHandler Loaded;
    event ConnectionErrorHandler BadConnectEvent;
    Task AddToCart(int? filialId = null);
}
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Enums;
using priceapp.Events.Delegates;
using priceapp.Models;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(CartProcessedViewModel))]

namespace priceapp.ViewModels;

public class CartProcessedViewModel : ICartProcessedViewModel
{
    public event LoadingHandler Loaded;
    public event ConnectionErrorHandler BadConnectEvent;
    public ObservableCollection<ItemToBuy> ItemsList { get; set; } = new();

    private readonly IMapper _mapper;

    public CartProcessedViewModel()
    {
        _mapper = DependencyService.Get<IMapper>();
    }

    public async Task LoadAsync(IList<ItemToBuy> items, CartProcessingType type)
    {
        
    }
}
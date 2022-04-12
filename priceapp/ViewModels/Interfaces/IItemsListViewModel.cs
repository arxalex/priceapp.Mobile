using System.Collections.Generic;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface IItemsListViewModel
{
    IList<Item> Items { get; set; }
    int CategoryId { get; set; }
    
    void Load(int page);
    IList<Item> GetItems();
}
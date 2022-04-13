using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using priceapp.Models;

namespace priceapp.ViewModels.Interfaces;

public interface IItemsListViewModel
{
    List<Item> Items { get; set; }
    int CategoryId { get; set; }
    void LoadAsync();
    void Reload();
}
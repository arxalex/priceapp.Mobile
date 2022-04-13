using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using priceapp.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ItemsListViewModel))]

namespace priceapp.ViewModels;

public class ItemsListViewModel : IItemsListViewModel
{
    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;
    private const int PageSize = 25;
    public List<Item> Items { get; set; }
    public int CategoryId { get; set; }

    public ItemsListViewModel()
    {
        Items = new List<Item>();
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
    }

    public void LoadAsync()
    {
        Items.AddRange(_mapper.Map<IList<ItemRepositoryModel>, IList<Item>>(_itemRepository.GetItems(CategoryId, 0, PageSize)));
    }

    public void Reload()
    {
        Items.Clear();
    }
}
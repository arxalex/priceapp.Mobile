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
    public IList<Item> Items { get; set; } = new List<Item>();
    public int CategoryId { get; set; }
    private const int PageSize = 25;

    private readonly IMapper _mapper;
    private readonly IItemRepository _itemRepository;

    public ItemsListViewModel()
    {
        _mapper = DependencyService.Get<IMapper>();
        _itemRepository = DependencyService.Get<IItemRepository>();
    }
    
    public void Load(int page)
    {
        var list = _itemRepository.GetItems(CategoryId, page * PageSize, (page + 1) * PageSize);
        var items = _mapper.Map<IList<ItemRepositoryModel>, IList<Item>>(list);
        var excited = Items.ToList();
        excited.AddRange(items);
        Items = excited;
    }

    public IList<Item> GetItems()
    {
        return Items;
    }
}
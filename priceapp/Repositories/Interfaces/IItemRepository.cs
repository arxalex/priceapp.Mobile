using System.Collections.Generic;
using priceapp.Models;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IItemRepository
{
    IList<ItemRepositoryModel> GetItems(int categoryId, int from, int to);
}
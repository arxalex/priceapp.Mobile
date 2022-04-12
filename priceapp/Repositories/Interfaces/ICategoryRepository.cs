using System.Collections.Generic;
using priceapp.Models;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface ICategoryRepository
{
    IList<CategoryRepositoryModel> GetCategories(int? parent = null);
}
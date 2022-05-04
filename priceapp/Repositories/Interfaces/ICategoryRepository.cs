using System.Collections.Generic;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<IList<CategoryRepositoryModel>> GetCategories(int? parent = null);
    event ConnectionErrorHandler BadConnectEvent;
}
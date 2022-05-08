using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface ICategoriesLocalRepository
{
    Task<int> AddCategory(CategoryLocalDatabaseModel model);
    Task RemoveCategory(int id);
    Task<List<CategoryLocalDatabaseModel>> GetCategories();
    Task<List<CategoryLocalDatabaseModel>> GetCategories(Expression<Func<CategoryLocalDatabaseModel, bool>> expression);
    Task UpdateCategory(ItemToBuyLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
}
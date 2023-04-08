using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface ICategoriesLocalRepository
{
    Task<int> InsertAsync(CategoryLocalDatabaseModel model);
    Task DeleteAsync(int id);
    Task<List<CategoryLocalDatabaseModel>> GetAsync();
    Task<List<CategoryLocalDatabaseModel>> GetAsync(Expression<Func<CategoryLocalDatabaseModel, bool>> expression);
    Task UpdateAsync(CategoryLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
    Task DeleteAllAsync();
}
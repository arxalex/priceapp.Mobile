using System.Linq.Expressions;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IBrandAlertsLocalRepository
{
    Task<int> InsertAsync(BrandAlertLocalDatabaseModel model);
    Task DeleteAsync(int id);
    Task<List<BrandAlertLocalDatabaseModel>> GetAsync();
    Task<List<BrandAlertLocalDatabaseModel>> GetAsync(Expression<Func<BrandAlertLocalDatabaseModel, bool>> expression);
    Task UpdateAsync(BrandAlertLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
    Task DeleteAllAsync();
}
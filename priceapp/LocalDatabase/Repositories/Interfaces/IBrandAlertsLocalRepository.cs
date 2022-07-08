using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IBrandAlertsLocalRepository
{
    Task<int> AddItem(BrandAlertLocalDatabaseModel model);
    Task RemoveItem(int id);
    Task<List<BrandAlertLocalDatabaseModel>> GetItems();
    Task<List<BrandAlertLocalDatabaseModel>> GetItems(Expression<Func<BrandAlertLocalDatabaseModel, bool>> expression);
    Task UpdateItem(BrandAlertLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
}
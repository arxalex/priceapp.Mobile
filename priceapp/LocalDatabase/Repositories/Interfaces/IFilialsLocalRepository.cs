using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.LocalDatabase.Models;

namespace priceapp.LocalDatabase.Repositories.Interfaces;

public interface IFilialsLocalRepository
{
    Task<int> AddFilial(FilialLocalDatabaseModel model);
    Task RemoveFilial(int id);
    Task<List<FilialLocalDatabaseModel>> GetFilials();
    Task<List<FilialLocalDatabaseModel>> GetFilials(Expression<Func<FilialLocalDatabaseModel, bool>> expression);
    Task UpdateFilial(FilialLocalDatabaseModel model);
    event ConnectionErrorHandler BadConnectEvent;
}
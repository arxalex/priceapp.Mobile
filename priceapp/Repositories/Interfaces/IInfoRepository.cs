using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IInfoRepository
{
    Task<InfoRepositoryModel> GetInfo();
    event ConnectionErrorHandler BadConnectEvent;
    Task<bool?> IsAppNeedUpdate();
}
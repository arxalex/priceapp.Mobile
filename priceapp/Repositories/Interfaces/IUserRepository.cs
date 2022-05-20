using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IUserRepository
{
    event ConnectionErrorHandler BadConnectEvent;
    Task<UserRepositoryModel> GetUser();
}
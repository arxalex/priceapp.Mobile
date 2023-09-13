using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IUserRepository
{
    event ConnectionErrorHandler BadConnectEvent;
    Task<UserRepositoryModel> GetUser();
    Task<LoginResultModel> Login(string username, string password);
    Task<bool> IsUserLoggedIn();
    Task<RegisterResultModel> Register(string username, string email, string password);
    Task<DeleteResultModel> Delete(string password);
}
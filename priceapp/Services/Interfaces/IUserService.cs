using System.Threading.Tasks;
using priceapp.Events.Models;

namespace priceapp.Services.Interfaces;

public interface IUserService
{
    Task<bool> IsUserLoggedIn();
    Task<ProcessedArgs> LoginUser(string username, string password);
    Task<ProcessedArgs> LoginAsGuest();
    Task<bool> IsUserWasLoggedIn();
    void LogoutUser();
    Task<ProcessedArgs> RegisterUser(string username, string email, string password);
}
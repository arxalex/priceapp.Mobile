using System.Threading.Tasks;
using priceapp.Events.Delegates;

namespace priceapp.ViewModels.Interfaces
{
    public interface ILoginViewModel
    {
        event LoginHandler LoginSuccess;
        Task LoginUser(string email, string password);
        bool IsUserLoggedIn();
    }
}
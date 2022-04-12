using System.Threading.Tasks;
using priceapp.Events.Delegates;

namespace priceapp.ViewModels.Interfaces
{
    public interface ILoginViewModel
    {
        event LoginHandler LoginSuccess;
        void LoginUser(string email, string password);
        Task<bool> IsUserLoggedInAsync();
    }
}
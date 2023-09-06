using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Services.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginViewModel))]

namespace priceapp.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly IUserService _userService = DependencyService.Get<IUserService>(DependencyFetchTarget.NewInstance);

        public event LoginHandler LoginSuccess;
        public async Task LoginUser(string email, string password)
        {
            LoginSuccess?.Invoke(this, await _userService.LoginUser(email, password));
        }

        public async Task LoginAsGuest()
        {
            LoginSuccess?.Invoke(this, await _userService.LoginAsGuest());
        }
    }
}
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Services.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(RegistrationViewModel))]

namespace priceapp.ViewModels
{
    public class RegistrationViewModel : IRegistrationViewModel
    {
        private readonly IUserService _userService = DependencyService.Get<IUserService>(DependencyFetchTarget.NewInstance);

        public event LoginHandler RegisterSuccess;

        public async Task RegisterUser(string username, string email, string password)
        {
            RegisterSuccess?.Invoke(this, await _userService.RegisterUser(username, email, password));
        }
    }
}
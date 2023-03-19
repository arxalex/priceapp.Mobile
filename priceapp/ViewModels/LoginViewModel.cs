using System.Linq;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Interfaces;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginViewModel))]

namespace priceapp.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IConnectionService _connectionService;

        public LoginViewModel()
        {
            _userRepository = DependencyService.Get<IUserRepository>();
            _connectionService = DependencyService.Get<IConnectionService>();
        }

        public event LoginHandler LoginSuccess;

        public async Task LoginUser(string username, string password)
        {
            if (await _connectionService.IsConnectedAsync())
            {
                LoginSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Відсутнє зʼєднання з сервером"});
                return;
            }
            
            if (username == null || password == null)
            {
                LoginSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Усі поля обов'язкові до заповнення"});
                return;
            }

            if (username.Contains('@'))
            {
                if (!StringUtil.IsValidEmail(username))
                {
                    LoginSuccess?.Invoke(this,
                        new ProcessedArgs() {Success = false, Message = "E-mail вказано некорректно"});
                    return;
                }
            }
            else
            {
                if (!StringUtil.IsValidUsername(username))
                {
                    LoginSuccess?.Invoke(this,
                        new ProcessedArgs()
                        {
                            Success = false,
                            Message =
                                "Ім'я користувача містить недопустимі символи. Використовуйте лише малі літери латинського алфавіту, цифри та символи \".\" і \"_\""
                        });
                    return;
                }
            }

            var loginStatus = await _userRepository.Login(username, password);

            if (!loginStatus.Succsess)
            {
                LoginSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = loginStatus.Message});
                return;
            }

            LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = true, Message = "Вхід виконано"});
        }

        public async Task LoginAsGuest()
        {
            const string username = "guest";
            const string password = "Anonymu?Password_doNotHackP12";

            await LoginUser(username, password);
        }

        public bool IsUserLoggedIn()
        {
            return !_connectionService.IsConnected() || _userRepository.IsUserLoggedIn().Result;
        }
    }
}
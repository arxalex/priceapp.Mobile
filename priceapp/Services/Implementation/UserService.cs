using System.Threading.Tasks;
using priceapp.Events.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using priceapp.Utils;
using priceapp.Views;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserService))]

namespace priceapp.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConnectionService _connectionService;

    public UserService()
    {
        _userRepository = DependencyService.Get<IUserRepository>(DependencyFetchTarget.NewInstance);
        _connectionService = DependencyService.Get<IConnectionService>();
    }

    public async Task<bool> IsUserLoggedIn()
    {
        return await _userRepository.IsUserLoggedIn();
    }

    public async Task<bool> IsUserWasLoggedIn()
    {
        return await ConnectionUtil.IsTokenExists();
    }

    public async Task<ProcessedArgs> LoginUser(string username, string password)
    {
        if (!await _connectionService.IsConnectedAsync())
        {
            return new ProcessedArgs { Success = false, Message = "Відсутнє зʼєднання з сервером" };
        }

        if (username == null || password == null)
        {
            return new ProcessedArgs { Success = false, Message = "Усі поля обов'язкові до заповнення" };
        }

        if (username.Contains("@"))
        {
            if (!StringUtil.IsValidEmail(username))
            {
                return new ProcessedArgs { Success = false, Message = "E-mail вказано некорректно" };
            }
        }
        else
        {
            if (!StringUtil.IsValidUsername(username))
            {
                return new ProcessedArgs
                {
                    Success = false,
                    Message =
                        "Ім'я користувача містить недопустимі символи. Використовуйте лише малі літери латинського алфавіту, цифри та символи \".\" і \"_\""
                };
            }
        }

        var loginStatus = await _userRepository.Login(username, password);

        if (!loginStatus.Succsess)
        {
            return new ProcessedArgs { Success = false, Message = loginStatus.Message };
        }

        return new ProcessedArgs { Success = true, Message = "Вхід виконано" };
    }

    public async Task<ProcessedArgs> LoginAsGuest()
    {
        const string username = "guest";
        const string password = "Anonymu?Password_doNotHackP12";

        return await LoginUser(username, password);
    }

    public void LogoutUser()
    {
        ConnectionUtil.RemoveToken();
        Xamarin.Essentials.SecureStorage.Remove("username");
        Xamarin.Essentials.SecureStorage.Remove("email");
        Application.Current.MainPage = new LoginPage();
    }
}
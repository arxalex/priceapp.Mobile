using System.Threading.Tasks;
using priceapp.Events.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using priceapp.Services.Models;
using priceapp.Utils;
using priceapp.Views;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserService))]

namespace priceapp.Services.Implementation;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository =
        DependencyService.Get<IUserRepository>(DependencyFetchTarget.NewInstance);

    private readonly IConnectionService _connectionService = DependencyService.Get<IConnectionService>();

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

    public async Task<ProcessedArgs> RegisterUser(string username, string email, string password)
    {
        if (username == null || password == null || email == null)
        {
            return new ProcessedArgs() { Success = false, Message = "Усі поля обов'язкові до заповнення" };
        }

        if (!StringUtil.IsValidUsername(username))
        {
            return new ProcessedArgs()
            {
                Success = false,
                Message =
                    "Ім'я користувача містить недопустимі символи. Використовуйте лише малі літери латинського алфавіту, цифри та символи \".\" і \"_\""
            };
        }

        if (!StringUtil.IsValidEmail(email))
        {
            return new ProcessedArgs() { Success = false, Message = "E-mail вказано некорректно" };
        }

        var loginStatus = await _userRepository.Register(username, email, password);

        if (!loginStatus.Succsess)
        {
            return new ProcessedArgs { Success = false, Message = loginStatus.Message };
        }

        return new ProcessedArgs { Success = true, Message = "Реєстрація успішна" };
    }

    public async Task<ProcessedArgs> DeleteAccountAsync(string password)
    {
        if (!await _connectionService.IsConnectedAsync())
        {
            return new ProcessedArgs { Success = false, Message = "Відсутнє зʼєднання з сервером" };
        }

        if (password == null)
        {
            return new ProcessedArgs { Success = false, Message = "Усі поля обов'язкові до заповнення" };
        }

        var loginStatus = await _userRepository.Delete(password);

        if (!loginStatus.Succsess)
        {
            return new ProcessedArgs { Success = false, Message = loginStatus.Message };
        }

        return new ProcessedArgs { Success = true, Message = "Видалення виконано" };
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
        ConnectionUtil.RemoveUserInfo();
        Application.Current.MainPage = new LoginPage();
    }

    public async Task<UserModel> GetUser()
    {
        var result = new UserModel();
        if (!await ConnectionUtil.IsUserInfoExists())
        {
            var user = await _userRepository.GetUser();

            await ConnectionUtil.UpdateUserInfo(user.username, user.email);

            result.Username = user.username;
            result.Email = user.email;
        }
        else
        {
            var userInfo = await ConnectionUtil.GetUserInfo();
            result.Username = userInfo.username;
            result.Email = userInfo.email;
        }

        return result;
    }
}
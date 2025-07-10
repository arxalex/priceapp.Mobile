using priceapp.Events.Delegates;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.ViewModels;

public class LoginViewModel : ILoginViewModel
{
    private readonly IUserService _userService;

    public LoginViewModel(IUserService userService)
    {
        _userService = userService;
    }

    public event LoginHandler? LoginSuccess;
    public async Task LoginUser(string email, string password)
    {
        LoginSuccess?.Invoke(this, await _userService.LoginUser(email, password));
    }

    public async Task LoginAsGuest()
    {
        LoginSuccess?.Invoke(this, await _userService.LoginAsGuest());
    }
}
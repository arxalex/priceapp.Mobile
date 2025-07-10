using priceapp.Events.Delegates;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.ViewModels;

public class RegistrationViewModel : IRegistrationViewModel
{
    private readonly IUserService _userService;

    public RegistrationViewModel(IUserService userService)
    {
        _userService = userService;
    }

    public event LoginHandler? RegisterSuccess;

    public async Task RegisterUser(string username, string email, string password)
    {
        RegisterSuccess?.Invoke(this, await _userService.RegisterUser(username, email, password));
    }
}
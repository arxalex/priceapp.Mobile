using priceapp.Events.Delegates;

namespace priceapp.ViewModels.Interfaces;

public interface IRegistrationViewModel
{
    event LoginHandler RegisterSuccess;
    Task RegisterUser(string username, string email, string password);
}
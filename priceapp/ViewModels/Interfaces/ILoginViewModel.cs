namespace priceapp.ViewModels.Interfaces
{
    public interface ILoginViewModel
    {
        bool LoginUser(string email, string password);
        bool IsUserLoggedIn();
    }
}
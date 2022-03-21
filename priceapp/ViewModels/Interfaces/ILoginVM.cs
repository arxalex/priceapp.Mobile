namespace priceapp.ViewModels.Interfaces
{
    public interface ILoginVM
    {
        bool LoginUser(string email, string password);
        bool IsUserLoggedIn();
    }
}
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly:Dependency(typeof(LoginVM))]
namespace priceapp.ViewModels
{
    public class LoginVM : ILoginVM
    {
        public bool LoginUser(string email, string password)
        {
            if (email != "" && password != "")
            {
                Application.Current.Properties["isLoggedIn"] = true;
                return true;
            }

            return false;
        }

        public bool IsUserLoggedIn()
        {
            if (Application.Current.Properties.ContainsKey("isLoggedIn"))
            {
                return (bool) Application.Current.Properties["isLoggedIn"];
            }

            Application.Current.Properties["isLoggedIn"] = false;
            return false;
        }
    }
}
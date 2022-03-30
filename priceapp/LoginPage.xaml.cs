using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private ILoginViewModel _loginViewModel;
        public LoginPage()
        {
            InitializeComponent();
            _loginViewModel = DependencyService.Get<ILoginViewModel>();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if (_loginViewModel.LoginUser(EntryEmail.Text, EntryPassword.Text))
            {
                Application.Current.MainPage = new MainPage();
            }
        }
    }
}
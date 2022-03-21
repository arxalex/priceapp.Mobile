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
        private ILoginVM _loginVM;
        public LoginPage()
        {
            InitializeComponent();
            _loginVM = DependencyService.Get<ILoginVM>();
        }

        private void Button_OnClicked(object sender, EventArgs e)
        {
            if (_loginVM.LoginUser(EntryEmail.Text, EntryPassword.Text))
            {
                Application.Current.MainPage = new MainPage();
            }
        }
    }
}
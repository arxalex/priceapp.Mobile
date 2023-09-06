using System;
using priceapp.Events.Models;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RegisterPage
{
    private readonly IRegistrationViewModel _registrationViewModel;

    public RegisterPage()
    {
        InitializeComponent();
        _registrationViewModel = DependencyService.Get<IRegistrationViewModel>(DependencyFetchTarget.NewInstance);
        _registrationViewModel.RegisterSuccess += RegistrationViewModelOnRegisterSuccess;
    }

    private void RegistrationViewModelOnRegisterSuccess(object sender, ProcessedArgs args)
    {
        if (args.Success)
        {
            Application.Current.MainPage = new NavigationPage(new ConfirmEmailPage());
        }
        else
        {
            ProcessedFrame.IsVisible = true;
            ProcessedLabel.Text = args.Message;
        }
    }

    private async void Button_OnClicked(object sender, EventArgs e)
    {
        await _registrationViewModel.RegisterUser(EntryUsername.Text, EntryEmail.Text, EntryPassword.Text);
    }

    private void ButtonLogin_OnClicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new LoginPage();
    }
}
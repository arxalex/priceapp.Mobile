using System;
using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace priceapp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DeleteAccountPage
    {
        private readonly IDeleteAccountViewModel _deleteAccountViewModel = DependencyService.Get<IDeleteAccountViewModel>(DependencyFetchTarget.NewInstance);
        private readonly IUserService _userService = DependencyService.Get<IUserService>(DependencyFetchTarget.NewInstance);

        public DeleteAccountPage()
        {
            InitializeComponent();
            BindingContext = _deleteAccountViewModel;
            
            ActivityIndicator.IsRunning = true;
            ActivityIndicator.IsVisible = true;
            
            _deleteAccountViewModel.DeleteSuccess += DeleteAccountViewModelOnDeleteAccountSuccess;
            _deleteAccountViewModel.Loaded += DeleteAccountViewModelOnLoaded;
            _deleteAccountViewModel.LoadAsync();
        }

        private void DeleteAccountViewModelOnLoaded(object sender, LoadingArgs args)
        {
            ActivityIndicator.IsRunning = false; 
            ActivityIndicator.IsVisible = false;
        }

        private async void DeleteAccountViewModelOnDeleteAccountSuccess(object sender, ProcessedArgs args)
        {
            if (args.Success)
            {
                var loginResult = await _userService.LoginAsGuest();
                if (!loginResult.Success)
                {
                    Application.Current.MainPage = new ConnectionErrorPage(new ConnectionErrorArgs()
                    {
                        Message = loginResult.Message,
                        StatusCode = 400,
                        Success = loginResult.Success
                    });
                }
                Application.Current.MainPage = new MainPage();
            }
            else
            {
                ProcessedFrame.IsVisible = true;
                ProcessedLabel.Text = args.Message;
            }
        }

        private async void Button_OnClicked(object sender, EventArgs e)
        {
            await _deleteAccountViewModel.DeleteUser(EntryPassword.Text);
        }

        private async void HeaderBackButton_OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
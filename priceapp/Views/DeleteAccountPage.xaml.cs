using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.Views;

public partial class DeleteAccountPage
{
    private readonly IDeleteAccountViewModel _deleteAccountViewModel;
    private readonly IUserService _userService;

    public DeleteAccountPage(IDeleteAccountViewModel deleteAccountViewModel, IUserService userService)
    {
        InitializeComponent();
        _deleteAccountViewModel = deleteAccountViewModel;
        _userService = userService;
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
                Application.Current.Windows[0].Page = new ConnectionErrorPage(new ConnectionErrorArgs
                {
                    Message = loginResult.Message,
                    StatusCode = 400,
                    Success = loginResult.Success
                });
            }
            Application.Current.Windows[0].Page = new MainPage();
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
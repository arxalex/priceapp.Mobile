using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

using MenuItem = priceapp.UI.MenuItem;

namespace priceapp.Views;

public partial class AccountPage
{
    private readonly IAccountViewModel _accountViewModel;
    private readonly IServiceProvider _serviceProvider;
    public AccountPage(IAccountViewModel accountViewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _accountViewModel = accountViewModel;
        _serviceProvider = serviceProvider;

        _accountViewModel.Loaded += AccountViewModelOnLoaded;
        _accountViewModel.BadConnectEvent += AccountViewModelOnBadConnectEvent;

        ActivityIndicator.IsRunning = true;
        ActivityIndicator.IsVisible = true;

        _accountViewModel.LoadAsync();

        BindingContext = _accountViewModel;
    }

    private void AccountViewModelOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        Navigation.PushAsync(new ConnectionErrorPage(args));
    }

    private void AccountViewModelOnLoaded(object sender, LoadingArgs args)
    {
        ActivityIndicator.IsRunning = false;
        ActivityIndicator.IsVisible = false;
    }

    private void CollectionView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (CollectionView.SelectedItem == null) return;
        var item = (MenuItem) e.CurrentSelection.FirstOrDefault()!;
        CollectionView.SelectedItem = null;
        switch (item.Label)
        {
            case "Налаштування":
                Navigation.PushAsync(new SettingPage(_serviceProvider.GetRequiredService<ISettingsViewModel>()));
                break;
            case "Про додаток":
                Navigation.PushAsync(new AboutPage());
                break;
            case "Змінити акаунт":
                _accountViewModel.ChangeAccount();
                break;
            case "Підказки":
                if (Application.Current != null) Application.Current.Windows[0].Page = new OnboardingPage();
                break;
            case "Питання та відповіді":
                Browser.OpenAsync("https://priceapp.co/documents/faq", BrowserLaunchMode.SystemPreferred);
                break;
            case "Політика конфіденційності":
                Browser.OpenAsync("https://priceapp.co/documents/privacy",
                    BrowserLaunchMode.SystemPreferred);
                break;
            case "Новини":
                Browser.OpenAsync("https://t.me/price_app", BrowserLaunchMode.SystemPreferred);
                break;
            case "Видалити акаунт":
                Navigation.PushAsync(new DeleteAccountPage(_serviceProvider.GetRequiredService<IDeleteAccountViewModel>(), _serviceProvider.GetRequiredService<IUserService>()));
                break;
        }

        
    }
}
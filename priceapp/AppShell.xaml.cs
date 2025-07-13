using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;
using priceapp.Views;

namespace priceapp;

public enum AppState
{
    Main,
    ConnectionError,
    Update,
    Onboarding
}

public partial class AppShell : Shell
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnectionService _connectionService;
    private readonly IUserService _userService;

    private AppState _currentState;
    public AppState CurrentState
    {
        get => _currentState;
        set
        {
            _currentState = value;
            OnPropertyChanged();
        }
    }

    private string _errorMessage;
    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    private int _statusCode;
    public int StatusCode
    {
        get => _statusCode;
        set
        {
            _statusCode = value;
            OnPropertyChanged();
        }
    }

    public AppShell(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _connectionService = serviceProvider.GetRequiredService<IConnectionService>();
        _userService = serviceProvider.GetRequiredService<IUserService>();
        _connectionService.BadConnectEvent += ConnectionServiceOnBadConnectEvent;
        
        BindingContext = this;
        InitializeAppState();
    }

    private async void InitializeAppState()
    {
        try
        {
            var isConnected = await _connectionService.IsConnectedAsync();
            if (!isConnected)
            {
                var isUserWasLoggedIn = await _userService.IsUserWasLoggedIn();
                if (isUserWasLoggedIn)
                {
                    CurrentState = AppState.Main;
                    return;
                }

                CurrentState = AppState.ConnectionError;
                ErrorMessage = "Відсутнє зʼєднання з сервером";
                StatusCode = 404;
                return;
            }

            var needUpdate = await _connectionService.IsAppNeedsUpdateAsync();
            if (needUpdate != null && needUpdate.Value)
            {
                CurrentState = AppState.Update;
                return;
            }

            var isLoggedIn = await _userService.IsUserLoggedIn();
            if (!isLoggedIn)
            {
                var loginResult = await _userService.LoginAsGuest();
                if (!loginResult.Success)
                {
                    CurrentState = AppState.ConnectionError;
                    ErrorMessage = loginResult.Message;
                    StatusCode = 400;
                    return;
                }
            }

            if (VersionTracking.IsFirstLaunchEver)
            {
                CurrentState = AppState.Onboarding;
                return;
            }

            CurrentState = AppState.Main;
        }
        catch (Exception ex)
        {
            CurrentState = AppState.ConnectionError;
            ErrorMessage = "Unexpected error occurred";
            StatusCode = 500;
        }
    }

    private void ConnectionServiceOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        CurrentState = AppState.ConnectionError;
        ErrorMessage = args.Message;
        StatusCode = args.StatusCode;
    }
}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels.Interfaces;

namespace priceapp.ViewModels;

public class DeleteAccountViewModel : IDeleteAccountViewModel
{
    private readonly IUserService _userService;
    private readonly ICacheService _cacheService;
    private string? _email;
    private string? _username;

    public DeleteAccountViewModel(
        IUserService userService,
        ICacheService cacheService
        ) {
        _userService = userService;
        _cacheService = cacheService;
    }

    public string? Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string? Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public event DeleteAccountHandler? DeleteSuccess;
    public event LoadingHandler? Loaded;
        
    public async Task LoadAsync()
    {
        var user = await _userService.GetUser();
        Username = "Імʼя користувача: " + user.Username;
        Email = "Email: " + user.Email;

        Loaded?.Invoke(this,
            new LoadingArgs {Success = true, LoadedCount = 1, Total = 1});
    }

    public async Task DeleteUser(string password)
    {
        await _cacheService.ClearAsync();
        DeleteSuccess?.Invoke(this, await _userService.DeleteAccountAsync(password));
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
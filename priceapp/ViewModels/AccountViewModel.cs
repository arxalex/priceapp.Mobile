using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using priceapp.Annotations;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;
using MenuItem = priceapp.UI.MenuItem;

[assembly: Xamarin.Forms.Dependency(typeof(AccountViewModel))]

namespace priceapp.ViewModels;

public class AccountViewModel : IAccountViewModel
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ICacheService _cacheService;
    private string _email;

    private string _username;

    public AccountViewModel()
    {
        _userRepository = DependencyService.Get<IUserRepository>(DependencyFetchTarget.NewInstance);
        _userService = DependencyService.Get<IUserService>();
        _cacheService = DependencyService.Get<ICacheService>();

        MenuItems.Add(new MenuItem {Label = "Налаштування", Glyph = "\ue8b8"});
        MenuItems.Add(new MenuItem {Label = "Підказки", Glyph = "\ue79a"});
        MenuItems.Add(new MenuItem {Label = "Новини", Glyph = "\ueb81"});
        MenuItems.Add(new MenuItem {Label = "Про додаток", Glyph = "\ue88e"});
        MenuItems.Add(new MenuItem {Label = "Питання та відповіді", Glyph = "\uf04c"});
        MenuItems.Add(new MenuItem {Label = "Політика конфіденційності", Glyph = "\uf0dc"});
        MenuItems.Add(new MenuItem {Label = "Змінити акаунт", Glyph = "\ue7ef"});

        _userRepository.BadConnectEvent += UserRepositoryOnBadConnectEvent;
    }

    public ObservableCollection<MenuItem> MenuItems { get; set; } = new();

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            _email = value;
            OnPropertyChanged();
        }
    }

    public async Task LoadAsync()
    {
        if (await Xamarin.Essentials.SecureStorage.GetAsync("username") is not {Length: > 0})
        {
            var user = await _userRepository.GetUser();

            await Xamarin.Essentials.SecureStorage.SetAsync("username", user.username);
            await Xamarin.Essentials.SecureStorage.SetAsync("email", user.email);

            Username = user.username;
            Email = user.email;
        }
        else
        {
            Username = await Xamarin.Essentials.SecureStorage.GetAsync("username");
            Email = await Xamarin.Essentials.SecureStorage.GetAsync("email");
        }

        Loaded?.Invoke(this,
            new LoadingArgs() {Success = true, LoadedCount = 1, Total = 1});
    }

    public async Task ChangeAccount()
    {
        await _cacheService.ClearAsync();
        _userService.LogoutUser();
    }

    public event PropertyChangedEventHandler PropertyChanged;
    public event ConnectionErrorHandler BadConnectEvent;
    public event LoadingHandler Loaded;

    private void UserRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
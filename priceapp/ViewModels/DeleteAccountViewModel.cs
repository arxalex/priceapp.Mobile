using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Services.Interfaces;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(DeleteAccountViewModel))]

namespace priceapp.ViewModels
{
    public class DeleteAccountViewModel : IDeleteAccountViewModel
    {
        private readonly IUserService _userService = DependencyService.Get<IUserService>(DependencyFetchTarget.NewInstance);
        private readonly ICacheService _cacheService = DependencyService.Get<ICacheService>();
        private string _email;
        private string _username;
        
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

        public event DeleteAccountHandler DeleteSuccess;
        public event LoadingHandler Loaded;
        
        public async Task LoadAsync()
        {
            var user = await _userService.GetUser();
            Username = "Імʼя користувача: " + user.Username;
            Email = "Email: " + user.Email;

            Loaded?.Invoke(this,
                new LoadingArgs() {Success = true, LoadedCount = 1, Total = 1});
        }

        public async Task DeleteUser(string password)
        {
            await _cacheService.ClearAsync();
            DeleteSuccess?.Invoke(this, await _userService.DeleteAccountAsync(password));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
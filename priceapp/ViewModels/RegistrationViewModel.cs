using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(RegistrationViewModel))]

namespace priceapp.ViewModels
{
    public class RegistrationViewModel : IRegistrationViewModel
    {
        private readonly RestClient _client;

        public RegistrationViewModel()
        {
            var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
            var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
            {
                BaseAddress = new Uri("https://priceapp.arxalex.co/")
            };

            _client = new RestClient(httpClient);
            var cookie = Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result;

            if (cookie is not {Length: > 0})
            {
                Xamarin.Essentials.SecureStorage.Remove("cookie");
                return;
            }

            var expires = int.Parse(cookie
                .Split(' ')
                .First(x => x.Contains("token_expires"))
                .Substring(13)
                .Trim('=', ';'));

            if (DateTimeOffset.Now.ToUnixTimeSeconds() > expires)
            {
                Xamarin.Essentials.SecureStorage.Remove("cookie");
            }
            else
            {
                _client.AddDefaultHeader("Cookie", cookie);
            }
        }

        public event LoginHandler RegisterSuccess;

        public async Task RegisterUser(string username, string email, string password)
        {
            if (username == null || password == null || email == null)
            {
                RegisterSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Fields are empty"});
                return;
            }

            var json = JsonSerializer.Serialize(new
            {
                username, email, password
            });

            var request = new RestRequest("be/register", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(json, "application/json");

            var response = await _client.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                RegisterSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Login or email already exists or invalid"});
                return;
            }

            if (response.Content == null)
            {
                RegisterSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Something went wrong"});
                return;
            }

            var result = JsonSerializer.Deserialize<UserRegisterParse>(response.Content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            if (result is not {StatusRegister: true})
            {
                RegisterSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Status of register is false"});
                return;
            }

            if (response.Headers == null)
            {
                RegisterSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Something went wrong"});
                return;
            }

            RegisterSuccess?.Invoke(this, new ProcessedArgs() {Success = true, Message = "Success register"});
        }

        private class UserRegisterParse
        {
            public bool StatusRegister { get; set; }
        }
    }
}
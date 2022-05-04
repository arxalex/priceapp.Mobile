using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Utils;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoginViewModel))]

namespace priceapp.ViewModels
{
    public class LoginViewModel : ILoginViewModel
    {
        private readonly RestClient _client;
        public event LoginHandler LoginSuccess;

        public LoginViewModel()
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

        public async Task LoginUser(string username, string password)
        {
            if (username == null || password == null)
            {
                LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Fields are empty"});
                return;
            }

            string json;
            if (username.Contains('@'))
            {
                var email = username;
                var jsonBody = new
                {
                    email, password
                };
                json = JsonSerializer.Serialize(jsonBody);
            }
            else
            {
                var jsonBody = new
                {
                    username, password
                };
                json = JsonSerializer.Serialize(jsonBody);
            }

            var request = new RestRequest("be/login", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(json, "application/json");

            var response = await _client.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                LoginSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Login or password is incorrect"});
                return;
            }

            if (response.Content == null)
            {
                LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Something went wrong"});
                return;
            }

            var result = JsonSerializer.Deserialize<UserLoginParse>(response.Content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            if (result is not {StatusLogin: true})
            {
                LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Status of login is false"});
                return;
            }

            if (response.Headers == null)
            {
                LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Something went wrong"});
                return;
            }

            var cookies = response.Headers.Where(x => x.Name?.ToLower() == "set-cookie");
            var cookieContainer = cookies
                .Select(cookie => CookieUtil.Parse(cookie.Value as string))
                .ToDictionary(thisCookie => thisCookie.key, thisCookie => thisCookie.value);

            await Xamarin.Essentials.SecureStorage.SetAsync("cookie", CookieUtil.DictionaryToCookieString(cookieContainer));

            LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = true, Message = "Success login"});
        }

        public bool IsUserLoggedIn()
        {
            var request = new RestRequest("be/login", Method.Post);

            request.AddHeader("Content-Type", "application/json");

            var response = _client.ExecuteAsync(request).Result;

            if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
            {
                return false;
            }

            var result = JsonSerializer.Deserialize<UserLoginParse>(response.Content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            return result is {StatusLogin: false};
        }

        private class UserLoginParse
        {
            public bool StatusLogin { get; set; }
            public int Role { get; set; }
        }
    }
}
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

        public event LoginHandler LoginSuccess;

        public async Task LoginUser(string username, string password)
        {
            if (username == null || password == null)
            {
                LoginSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Усі поля обов'язкові до заповнення"});
                return;
            }

            string json;
            if (username.Contains('@'))
            {
                var email = username;
                if (!StringUtil.IsValidEmail(email))
                {
                    LoginSuccess?.Invoke(this,
                        new ProcessedArgs() {Success = false, Message = "E-mail вказано некорректно"});
                    return;
                }

                var jsonBody = new
                {
                    email, password
                };
                json = JsonSerializer.Serialize(jsonBody);
            }
            else
            {
                if (!StringUtil.IsValidUsername(username))
                {
                    LoginSuccess?.Invoke(this,
                        new ProcessedArgs()
                        {
                            Success = false,
                            Message =
                                "Ім'я користувача містить недопустимі символи. Використовуйте лише малі літери латинського алфавіту, цифри та символи \".\" і \"_\""
                        });
                    return;
                }

                var jsonBody = new
                {
                    username, password
                };
                json = JsonSerializer.Serialize(jsonBody);
            }

            var request = new RestRequest("be/login", Method.Post);

            Xamarin.Essentials.SecureStorage.Remove("cookie");
            Xamarin.Essentials.SecureStorage.Remove("username");
            Xamarin.Essentials.SecureStorage.Remove("email");
            _client.DefaultParameters.RemoveParameter("Cookie");
            request.AddHeader("Content-Type", "application/json");
            request.AddBody(json, "application/json");

            var response = await _client.ExecuteAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                LoginSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Логін або пароль невірні"});
                return;
            }

            if (response.Content == null)
            {
                LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Щось пішло не так"});
                return;
            }

            var result = JsonSerializer.Deserialize<UserLoginParse>(response.Content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            if (result is not {StatusLogin: true})
            {
                LoginSuccess?.Invoke(this,
                    new ProcessedArgs() {Success = false, Message = "Вхід відхилено. Перевірте дані"});
                return;
            }

            if (response.Headers == null)
            {
                LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = false, Message = "Щось пішло не так"});
                return;
            }

            var cookies = response.Headers.Where(x => x.Name?.ToLower() == "set-cookie");
            var cookieContainer = cookies
                .Select(cookie => CookieUtil.Parse(cookie.Value as string))
                .ToDictionary(thisCookie => thisCookie.key, thisCookie => thisCookie.value);

            await Xamarin.Essentials.SecureStorage.SetAsync("cookie",
                CookieUtil.DictionaryToCookieString(cookieContainer));

            LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = true, Message = "Вхід виконано"});
        }

        public async Task LoginAsGuest()
        {
            const string username = "guest";
            const string password = "Anonymu?Password_doNotHackP12";

            await LoginUser(username, password);
        }

        public bool IsUserLoggedIn()
        {
            var request = new RestRequest("be/login", Method.Post);

            request.AddHeader("Content-Type", "application/json");

            var response = _client.ExecuteAsync(request).Result;

            if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
            {
                _client.DefaultParameters.RemoveParameter("Cookie");
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
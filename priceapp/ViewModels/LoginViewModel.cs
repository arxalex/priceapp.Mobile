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
        public event LoginHandler LoginSuccess;

        public void LoginUser(string username, string password)
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


            var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();

            var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
            {
                BaseAddress = new Uri("https://priceapp.arxalex.co/")
            };

            var client = new RestClient(httpClient);
            var request = new RestRequest("be/login", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(json, "application/json");

            var response = client.ExecuteAsync(request).Result;
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

            if (result == null || !result.StatusLogin)
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

            Xamarin.Essentials.SecureStorage.SetAsync("cookie", CookieUtil.DictionaryToCookieString(cookieContainer));

            Xamarin.Essentials.SecureStorage.SetAsync("isLoggedIn", "true");

            LoginSuccess?.Invoke(this, new ProcessedArgs() {Success = true, Message = "Success login"});
        }

        public async Task<bool> IsUserLoggedInAsync()
        {
            return await Xamarin.Essentials.SecureStorage.GetAsync("isLoggedIn") != null &&
                   bool.Parse(await Xamarin.Essentials.SecureStorage.GetAsync("isLoggedIn"));
        }

        class UserLoginParse
        {
            public bool StatusLogin { get; set; }
            public int Role { get; set; }
        }
    }
}
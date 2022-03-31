using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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
        public bool LoginUser(string username, string password)
        {
            if (username == "" || password == "")
            {
                return false;
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
            
            
            IPriceAppWebAccess priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();

            var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler()) {
                BaseAddress = new Uri("https://priceapp.arxalex.co/")  
            };
            
            var client = new RestClient(httpClient); 
            var request = new RestRequest("be/login", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(json, "application/json");

            var response = client.ExecuteAsync(request).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            if (response.Content == null)
            {
                return false;
            }
            
            var result = JsonSerializer.Deserialize<UserLoginParse>(response.Content, new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

            if (result == null || !result.StatusLogin)
            {
                return false;
            }
            
            if (response.Headers == null) {return false;}
            
            var cookies = response.Headers.Where(x => x.Name?.ToLower() == "set-cookie");
            foreach (var cookie in cookies)
            {
                var thisCookie = CookieUtil.Parse(cookie.Value as string);
                Application.Current.Properties[thisCookie.key] = thisCookie.value;
            }

            Application.Current.Properties["isLoggedIn"] = true;

            return true;
        }

        public bool IsUserLoggedIn()
        {
            if (Application.Current.Properties.ContainsKey("isLoggedIn"))
            {
                return (bool) Application.Current.Properties["isLoggedIn"];
            }

            Application.Current.Properties["isLoggedIn"] = false;
            return false;
        }
        
        class UserLoginParse
        {
            public bool StatusLogin { get; set; }
            public int Role { get; set; } 
        }
    }
}
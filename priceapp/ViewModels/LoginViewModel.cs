using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
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

            var jsonBody = new
            {
                username, password
            };
            IPriceAppWebAccess priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();

            var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler()) {
                BaseAddress = new Uri("https://priceapp.arxalex.co/")  
            };

            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            var client = new RestClient(httpClient);
            //client.Options.MaxTimeout = 30000;
            //client.Options.RemoteCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            var request = new RestRequest("be/login", Method.Post);

            request.AddHeader("Content-Type", "application/json");
            request.AddBody(JsonSerializer.Serialize(jsonBody), "application/json");

            var response = client.ExecuteAsync(request).Result;
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return false;
            }

            var cookies = response.Cookies;

            if (response.Content == null)
            {
                return false;
            }

            var result = JsonSerializer.Deserialize<(bool statusLogin, int role)>(response.Content);

            if (result.statusLogin != true)
            {
                return false;
            }

            Application.Current.Properties["isLoggedIn"] = true;
            Application.Current.Properties["Cookies"] = cookies;

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
    }
}
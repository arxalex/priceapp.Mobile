using System;
using System.Net.Http;
using System.Threading.Tasks;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

namespace priceapp.Utils;

public static class ConnectionUtil
{
    private static void SetToken(this RestClient client)
    {
        var token = Xamarin.Essentials.SecureStorage.GetAsync("token").Result;
        if (!string.IsNullOrEmpty(token))
        {
            client.AddDefaultHeader("Authorization", $"Bearer {token}");
        }
    }

    public static RestClient GetRestClient()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri(Constants.ApiUrl)
        };
        RestClient client = new RestClient(httpClient);

        client.SetToken();
        return client;
    }

    public static async Task UpdateToken(string token)
    {
        await Xamarin.Essentials.SecureStorage.SetAsync("token", token);
    }

    public static void RemoveToken()
    {
        Xamarin.Essentials.SecureStorage.Remove("token");
    }

    public static async Task<bool> IsTokenExists()
    {
        return !string.IsNullOrEmpty(await Xamarin.Essentials.SecureStorage.GetAsync("token"));
    }
}
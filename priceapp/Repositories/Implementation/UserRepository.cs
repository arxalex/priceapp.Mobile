using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserRepository))]

namespace priceapp.Repositories.Implementation;

public class UserRepository : IUserRepository
{
    private readonly RestClient _client;

    public UserRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")
        };
        _client = new RestClient(httpClient);
        _client.AddDefaultHeader("Cookie", Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result);
    }

    public event ConnectionErrorHandler BadConnectEvent;

    public async Task<UserRepositoryModel> GetUser()
    {
        var json = JsonSerializer.Serialize(new {source = 0});

        var request = new RestRequest("be/user/get_user_info", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return null;
        }

        return JsonSerializer.Deserialize<UserRepositoryModel>(response.Content);
    }
}
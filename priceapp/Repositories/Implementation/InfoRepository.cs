using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(InfoRepository))]

namespace priceapp.Repositories.Implementation;

public class InfoRepository : IInfoRepository
{
    private readonly RestClient _client;

    public InfoRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri(Constants.ApiUrl)
        };
        _client = new RestClient(httpClient);

        _client.AddDefaultHeader("Cookie", $"Bearer {Xamarin.Essentials.SecureStorage.GetAsync("token").Result}");
    }
    

    public async Task<InfoRepositoryModel> GetInfo()
    {
        var request = new RestRequest("Info");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<InfoRepositoryModel>(response.Content);
    }
}
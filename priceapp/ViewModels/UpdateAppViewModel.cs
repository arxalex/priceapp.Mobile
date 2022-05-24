using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using priceapp.ViewModels;
using priceapp.ViewModels.Interfaces;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(UpdateAppViewModel))]

namespace priceapp.ViewModels;

public class UpdateAppViewModel : IUpdateAppViewModel
{
    private readonly RestClient _client;

    public UpdateAppViewModel()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")
        };

        _client = new RestClient(httpClient);
    }


    public bool IsAppNeedUpdate()
    {
        var request = new RestRequest("be/check_version", Method.Post);

        var json = JsonSerializer.Serialize(new {installed = Constants.Version});

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = _client.ExecuteAsync(request).Result;

        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            return false;
        }

        var result = JsonSerializer.Deserialize<bool>(response.Content, new JsonSerializerOptions());

        return !result;
    }
}
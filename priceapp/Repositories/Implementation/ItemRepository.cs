using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(ItemRepository))]
namespace priceapp.Repositories.Implementation;

public class ItemRepository : IItemRepository
{
    private readonly RestClient _client;
    public ItemRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler()) {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")  
        };
        _client = new RestClient(httpClient);
        _client.AddDefaultHeader("Cookie", Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result);
    }
    public IList<ItemRepositoryModel> GetItems(int categoryId, int from, int to)
    {
        var request = new RestRequest("be/items/get_items", Method.Post);

        var json = JsonSerializer.Serialize(new { source = 0, category = categoryId, from, to, method = "viewModelByCategory" });
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = _client.ExecuteAsync(request).Result;
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            return new List<ItemRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<ItemRepositoryModel>>(response.Content) ?? new List<ItemRepositoryModel>();
        return list;
    }
}
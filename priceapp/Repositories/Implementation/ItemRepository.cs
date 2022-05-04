using System;
using System.Collections.Generic;
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

[assembly: Dependency(typeof(ItemRepository))]

namespace priceapp.Repositories.Implementation;

public class ItemRepository : IItemRepository
{
    private readonly RestClient _client;
    public event ConnectionErrorHandler BadConnectEvent;

    public ItemRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")
        };
        _client = new RestClient(httpClient);
        _client.AddDefaultHeader("Cookie", Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result);
    }

    public async Task<IList<ItemRepositoryModel>> GetItems(int categoryId,
        int from,
        int to,
        double? xCord = null,
        double? yCord = null,
        double? radius = null)
    {
        var request = new RestRequest("be/items/get_items", Method.Post);

        string json;
        if (xCord == null || yCord == null || radius == null)
        {
            json = JsonSerializer.Serialize(new
                {source = 0, category = categoryId, from, to, method = "viewModelByCategory"});
        }
        else
        {
            json = JsonSerializer.Serialize(new
            {
                source = 0, category = categoryId, from, to, method = "viewModelByCategoryAndLocation", xCord, yCord,
                radius
            });
        }

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs(){Success = false, StatusCode = (int) response.StatusCode});
            return new List<ItemRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<ItemRepositoryModel>>(response.Content) ??
                   new List<ItemRepositoryModel>();
        return list;
    }

    public async Task<ItemRepositoryModel> GetItem(int itemId,
        double? xCord = null,
        double? yCord = null,
        double? radius = null)
    {
        var request = new RestRequest("be/items/get_item", Method.Post);

        string json;
        if (xCord == null || yCord == null || radius == null)
        {
            json = JsonSerializer.Serialize(new {source = 0, id = itemId, method = "viewModelById"});
        }
        else
        {
            json = JsonSerializer.Serialize(new
            {
                source = 0, id = itemId, method = "viewModelByIdAndLocation", xCord, yCord, radius
            });
        }

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);

        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs(){Success = false, StatusCode = (int) response.StatusCode});
            return null;
        }

        return JsonSerializer.Deserialize<ItemRepositoryModel>(response.Content);
    }

    public async Task<IList<PriceAndFilialRepositoryModel>> GetPricesAndFilials(int itemId, double xCord, double yCord, int radius)
    {
        var request = new RestRequest("be/items/get_item", Method.Post);

        var json = JsonSerializer.Serialize(new
        {
            source = 0,
            id = itemId,
            xCord,
            yCord,
            radius,
            method = "pricesAndFilialsViewModelById"
        });
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs(){Success = false, StatusCode = (int) response.StatusCode});
            return new List<PriceAndFilialRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<PriceAndFilialRepositoryModel>>(response.Content) ??
                   new List<PriceAndFilialRepositoryModel>();
        return list;
    }
}
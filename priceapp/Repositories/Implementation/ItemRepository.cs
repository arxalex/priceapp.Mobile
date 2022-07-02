using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using priceapp.Enums;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
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
    private readonly ICacheRequestsLocalRepository _cacheRequestsLocalRepository;
    private readonly RestClient _client;
    private readonly IItemsLocalRepository _itemsLocalRepository;
    private readonly IMapper _mapper;

    public ItemRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
        _itemsLocalRepository = DependencyService.Get<IItemsLocalRepository>();
        _mapper = DependencyService.Get<IMapper>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")
        };
        _client = new RestClient(httpClient);
        _client.AddDefaultHeader("Cookie", Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result);
    }

    public event ConnectionErrorHandler BadConnectEvent;

    public async Task<IList<ItemRepositoryModel>> GetItems(int categoryId,
        int from,
        int to,
        double? xCord = null,
        double? yCord = null,
        double? radius = null)
    {
        string json;
        string requestProperty;
        if (xCord == null || yCord == null || radius == null)
        {
            json = JsonSerializer.Serialize(new
                {source = 0, category = categoryId, from, to, method = "viewModelByCategory"});
            requestProperty = json;
        }
        else
        {
            json = JsonSerializer.Serialize(new
            {
                source = 0, category = categoryId, from, to, method = "viewModelByCategoryAndLocation", xCord, yCord,
                radius
            });
            requestProperty = JsonSerializer.Serialize(new
            {
                source = 0,
                category = categoryId,
                from,
                to,
                method = "viewModelByCategoryAndLocation",
                xCord = Math.Round((double) xCord, 3),
                yCord = Math.Round((double) yCord, 3),
                radius
            });
        }

        if (await _cacheRequestsLocalRepository.Exists("be/items/get_items", requestProperty))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetCacheRecords(x =>
                    x.RequestName == "be/items/get_items" &&
                    x.RequestProperties == requestProperty &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _itemsLocalRepository
                .GetItems(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<ItemRepositoryModel>>(responseCache);
        }

        var request = new RestRequest("be/items/get_items", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return new List<ItemRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<ItemRepositoryModel>>(response.Content) ??
                   new List<ItemRepositoryModel>();

        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _itemsLocalRepository.AddItem(_mapper.Map<ItemLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.AddCacheRecord(new CacheRequestsLocalDatabaseModel
        {
            RequestName = "be/items/get_items",
            RequestProperties = requestProperty,
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(5)
        });

        return list;
    }

    public async Task<IList<ItemRepositoryModel>> SearchItems(string search,
        int from,
        int to,
        double? xCord = null,
        double? yCord = null,
        double? radius = null)
    {
        string json;
        string requestProperty;
        if (xCord == null || yCord == null || radius == null)
        {
            json = JsonSerializer.Serialize(new
            {
                source = 0,
                search, from, to, method = "viewModelBySearch"
            });
            requestProperty = json;
        }
        else
        {
            json = JsonSerializer.Serialize(new
            {
                source = 0,
                search, from, to, method = "viewModelBySearchAndLocation", xCord, yCord,
                radius
            });
            requestProperty = JsonSerializer.Serialize(new
            {
                source = 0,
                search,
                from,
                to,
                method = "viewModelBySearchAndLocation",
                xCord = Math.Round((double) xCord, 3),
                yCord = Math.Round((double) yCord, 3),
                radius
            });
        }

        if (await _cacheRequestsLocalRepository.Exists("be/items/get_items", requestProperty))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetCacheRecords(x =>
                    x.RequestName == "be/items/get_items" &&
                    x.RequestProperties == requestProperty &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _itemsLocalRepository
                .GetItems(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<ItemRepositoryModel>>(responseCache);
        }

        var request = new RestRequest("be/items/get_items", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return new List<ItemRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<ItemRepositoryModel>>(response.Content) ??
                   new List<ItemRepositoryModel>();

        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _itemsLocalRepository.AddItem(_mapper.Map<ItemLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.AddCacheRecord(new CacheRequestsLocalDatabaseModel
        {
            RequestName = "be/items/get_items",
            RequestProperties = requestProperty,
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(5)
        });

        return list;
    }

    public async Task<ItemRepositoryModel> GetItem(int itemId,
        double? xCord = null,
        double? yCord = null,
        double? radius = null)
    {
        string json;
        string requestProperty;
        if (xCord == null || yCord == null || radius == null)
        {
            json = JsonSerializer.Serialize(new {source = 0, id = itemId, method = "viewModelById"});
            requestProperty = json;
        }
        else
        {
            json = JsonSerializer.Serialize(new
            {
                source = 0, id = itemId, method = "viewModelByIdAndLocation", xCord, yCord, radius
            });
            requestProperty = JsonSerializer.Serialize(new
            {
                source = 0,
                id = itemId,
                method = "viewModelByIdAndLocation",
                xCord = Math.Round((double) xCord, 3),
                yCord = Math.Round((double) yCord, 3),
                radius
            });
        }

        if (await _cacheRequestsLocalRepository.Exists("be/items/get_item", requestProperty))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetCacheRecords(x =>
                    x.RequestName == "be/items/get_item" &&
                    x.RequestProperties == requestProperty &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _itemsLocalRepository
                .GetItems(x => responseCacheIds.Contains(x.RecordId));

            if (responseCache.Count != 1)
            {
                return null;
            }

            return _mapper.Map<ItemRepositoryModel>(responseCache.First());
        }

        var request = new RestRequest("be/items/get_item", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);

        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return null;
        }

        var item = JsonSerializer.Deserialize<ItemRepositoryModel>(response.Content);

        var recordIds = new List<int> {await _itemsLocalRepository.AddItem(_mapper.Map<ItemLocalDatabaseModel>(item))};

        await _cacheRequestsLocalRepository.AddCacheRecord(new CacheRequestsLocalDatabaseModel
        {
            RequestName = "be/items/get_item",
            RequestProperties = requestProperty,
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(5)
        });

        return item;
    }

    public async Task<IList<PriceAndFilialRepositoryModel>> GetPricesAndFilials(int itemId, double xCord, double yCord,
        int radius)
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
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return new List<PriceAndFilialRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<PriceAndFilialRepositoryModel>>(response.Content) ??
                   new List<PriceAndFilialRepositoryModel>();
        return list;
    }

    public async Task<(IList<PriceAndFilialRepositoryModel>, double)> GetShoppingList(
        List<ItemToBuyRepositoryModel> items,
        double xCord, double yCord,
        int radius, CartProcessingType type)
    {
        var request = new RestRequest("be/items/get_shopping_list", Method.Post);

        var method = "";

        switch (type)
        {
            case CartProcessingType.OneMarket:
                break;
            case CartProcessingType.OneMarketLowest:
                method = "oneLowest";
                break;
            case CartProcessingType.MultipleMarketsLowest:
                method = "multipleLowest";
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

        var json = JsonSerializer.Serialize(new
        {
            xCord,
            yCord,
            radius,
            method,
            items
        });
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return (new List<PriceAndFilialRepositoryModel>(), 0);
        }

        var content = JsonSerializer.Deserialize<ShoppingListResponse>(response.Content, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        }) ?? new ShoppingListResponse()
        {
            ShoppingList = new List<PriceAndFilialRepositoryModel>(),
            Economy = 0
        };

        return (content.ShoppingList, content.Economy);
    }

    private class ShoppingListResponse
    {
        public List<PriceAndFilialRepositoryModel> ShoppingList { get; set; } = new();
        public double Economy { get; set; }
    }
}
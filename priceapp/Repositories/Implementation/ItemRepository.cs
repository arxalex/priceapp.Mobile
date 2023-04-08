using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using priceapp.Utils;
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
        _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
        _itemsLocalRepository = DependencyService.Get<IItemsLocalRepository>();
        _mapper = DependencyService.Get<IMapper>();
        _client = ConnectionUtil.GetRestClient();
    }

    public event ConnectionErrorHandler BadConnectEvent;

    public async Task<IList<ItemRepositoryModel>> GetItems(int categoryId,
        int from,
        int to,
        double? xCord = null,
        double? yCord = null,
        double? radius = null)
    {
        string json = null;
        string requestUrl;
        string requestProperty;
        if (xCord == null || yCord == null || radius == null)
        {
            requestUrl = $"Items/category/{categoryId}/extended";
            requestProperty = JsonSerializer.Serialize(new
            {
                from,
                to
            });
        }
        else
        {
            requestUrl = $"Items/category/{categoryId}/location/extended";
            json = JsonSerializer.Serialize(new
            {
                xCord,
                yCord,
                radius
            });
            requestProperty = JsonSerializer.Serialize(new
            {
                from,
                to,
                xCord = Math.Round((double)xCord, 3),
                yCord = Math.Round((double)yCord, 3),
                radius
            });
        }

        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, requestProperty))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == requestProperty &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _itemsLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<ItemRepositoryModel>>(responseCache);
        }

        var request = new RestRequest(requestUrl, json != null ? Method.Post : Method.Get);

        request.AddHeader("Content-Type", "application/json");
        request.AddQueryParameter("from", from);
        request.AddQueryParameter("to", to);
        if (json != null)
        {
            request.AddBody(json, "application/json");
        }

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() { Success = false, StatusCode = (int)response.StatusCode });
            return new List<ItemRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<ItemRepositoryModel>>(response.Content) ??
                   new List<ItemRepositoryModel>();

        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _itemsLocalRepository.InsertAsync(_mapper.Map<ItemLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.InsertAsync(new CacheRequestsLocalDatabaseModel
        {
            RequestName = requestUrl,
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
        string requestUrl;

        if (xCord == null || yCord == null || radius == null)
        {
            requestUrl = "Items/search/extended";
            json = JsonSerializer.Serialize(new
            {
                search
            });
            requestProperty = JsonSerializer.Serialize(new
            {
                search,
                from,
                to
            });
        }
        else
        {
            requestUrl = "Items/search/location/extended";
            json = JsonSerializer.Serialize(new
            {
                search,
                xCord,
                yCord,
                radius
            });
            requestProperty = JsonSerializer.Serialize(new
            {
                search,
                from,
                to,
                xCord = Math.Round((double)xCord, 3),
                yCord = Math.Round((double)yCord, 3),
                radius
            });
        }

        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, requestProperty))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == requestProperty &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _itemsLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<ItemRepositoryModel>>(responseCache);
        }

        var request = new RestRequest(requestUrl, Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() { Success = false, StatusCode = (int)response.StatusCode });
            return new List<ItemRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<ItemRepositoryModel>>(response.Content) ??
                   new List<ItemRepositoryModel>();

        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _itemsLocalRepository.InsertAsync(_mapper.Map<ItemLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.InsertAsync(new CacheRequestsLocalDatabaseModel
        {
            RequestName = requestUrl,
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
        string json = null;
        string requestProperty;
        string requestUrl;

        if (xCord == null || yCord == null || radius == null)
        {
            requestUrl = $"Items/{itemId}/extended";
            requestProperty = "";
        }
        else
        {
            requestUrl = $"Items/{itemId}/location/extended";
            json = JsonSerializer.Serialize(new
            {
                xCord,
                yCord,
                radius
            });
            requestProperty = JsonSerializer.Serialize(new
            {
                xCord = Math.Round((double)xCord, 3),
                yCord = Math.Round((double)yCord, 3),
                radius
            });
        }

        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, requestProperty))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == requestProperty &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _itemsLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            if (responseCache.Count != 1)
            {
                return null;
            }

            return _mapper.Map<ItemRepositoryModel>(responseCache.First());
        }

        var request = new RestRequest(requestUrl, json != null ? Method.Post : Method.Get);

        request.AddHeader("Content-Type", "application/json");
        if (json != null)
        {
            request.AddBody(json, "application/json");
        }

        var response = await _client.ExecuteAsync(request);

        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() { Success = false, StatusCode = (int)response.StatusCode });
            return null;
        }

        var item = JsonSerializer.Deserialize<ItemRepositoryModel>(response.Content);

        var recordIds = new List<int>
            { await _itemsLocalRepository.InsertAsync(_mapper.Map<ItemLocalDatabaseModel>(item)) };

        await _cacheRequestsLocalRepository.InsertAsync(new CacheRequestsLocalDatabaseModel
        {
            RequestName = requestUrl,
            RequestProperties = requestProperty,
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(5)
        });

        return item;
    }
    
    public async Task<IList<PriceRepositoryModel>> GetPricesAndFilials(int itemId, double xCord, double yCord,
        int radius)
    {
        var requestUrl = $"Prices/{itemId}/location";
        var request = new RestRequest(requestUrl, Method.Post);

        var json = JsonSerializer.Serialize(new
        {
            xCord,
            yCord,
            radius
        });
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() { Success = false, StatusCode = (int)response.StatusCode });
            return new List<PriceRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<PriceRepositoryModel>>(response.Content) ??
                   new List<PriceRepositoryModel>();
        return list;
    }
    
    public async Task<(IList<PriceRepositoryModel>, double)> GetShoppingList(
        List<ShoppingListRepositoryModel> items,
        double xCord, double yCord,
        int radius, CartProcessingType type)
    {
        var request = new RestRequest("ShoppingList/location", Method.Post);

        var json = JsonSerializer.Serialize(new
        {
            xCord,
            yCord,
            radius,
            items
        });
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");
        request.AddQueryParameter("type", type);

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() { Success = false, StatusCode = (int)response.StatusCode });
            return (new List<PriceRepositoryModel>(), 0);
        }

        var content = JsonSerializer.Deserialize<ShoppingListResponse>(response.Content) ?? new ShoppingListResponse()
        {
            shoppingList = new List<PriceRepositoryModel>(),
            economy = 0,
            itemIdsNotFound = new List<int>()
        };

        return (content.shoppingList, content.economy);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
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

[assembly: Dependency(typeof(ShopRepository))]
namespace priceapp.Repositories.Implementation;

public class ShopRepository : IShopRepository
{
    public event ConnectionErrorHandler BadConnectEvent;
    private readonly RestClient _client;
    private readonly ICacheRequestsLocalRepository _cacheRequestsLocalRepository;
    private readonly IShopsLocalRepository _shopsLocalRepository;
    private readonly IFilialsLocalRepository _filialsLocalRepository;
    private readonly IMapper _mapper;

    public ShopRepository()
    {
        _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
        _shopsLocalRepository = DependencyService.Get<IShopsLocalRepository>();
        _filialsLocalRepository = DependencyService.Get<IFilialsLocalRepository>();
        _mapper = DependencyService.Get<IMapper>();
        _client = ConnectionUtil.GetRestClient();
    }

    public async Task<IList<ShopRepositoryModel>> GetShops()
    {
        const string requestUrl = "Shops";
        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, ""))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == "" &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _shopsLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<ShopRepositoryModel>>(responseCache);
        }
        
        var request = new RestRequest(requestUrl);

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs(){Success = false, StatusCode = (int) response.StatusCode});
            return new List<ShopRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<ShopRepositoryModel>>(response.Content) ?? new List<ShopRepositoryModel>();
        list.RemoveAll(x => x.id == 0);
        
        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _shopsLocalRepository.InsertAsync(_mapper.Map<ShopLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.InsertAsync(new CacheRequestsLocalDatabaseModel
        {
            RequestName = requestUrl,
            RequestProperties = "",
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(4080)
        });
        
        return list;
    }

    public async Task<IList<FilialRepositoryModel>> GetFilials()
    {
        const string requestUrl = "Filials";
        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, ""))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == "" &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _filialsLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<FilialRepositoryModel>>(responseCache);
        }
        
        var request = new RestRequest(requestUrl);
        
        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs(){Success = false, StatusCode = (int) response.StatusCode});
            return new List<FilialRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<FilialRepositoryModel>>(response.Content) ?? new List<FilialRepositoryModel>();
        list.RemoveAll(x => x.id == 0);
        
        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _filialsLocalRepository.InsertAsync(_mapper.Map<FilialLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.InsertAsync(new CacheRequestsLocalDatabaseModel
        {
            RequestName = requestUrl,
            RequestProperties = "",
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(480)
        });
        
        return list;
    }

    // TODO: Add location method
    public async Task<IList<FilialRepositoryModel>> GetFilialsAround(double xCord, double yCord, int radius)
    {
        const string requestUrl = "Filials/location";
        var json = JsonSerializer.Serialize(new
        {
            xCord,
            yCord,
            radius
        });

        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, json))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == json &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _filialsLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<FilialRepositoryModel>>(responseCache);
        }
        
        var request = new RestRequest(requestUrl, Method.Post);
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");
        
        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs(){Success = false, StatusCode = (int) response.StatusCode});
            return new List<FilialRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<FilialRepositoryModel>>(response.Content) ?? new List<FilialRepositoryModel>();
        list.RemoveAll(x => x.id == 0);
        
        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _filialsLocalRepository.InsertAsync(_mapper.Map<FilialLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.InsertAsync(new CacheRequestsLocalDatabaseModel
        {
            RequestName = requestUrl,
            RequestProperties = json,
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(480)
        });
        
        return list;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
using priceapp.WebServices;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(BrandAlertRepository))]

namespace priceapp.Repositories.Implementation;

public class BrandAlertRepository : IBrandAlertRepository
{
    private readonly IBrandAlertsLocalRepository _brandAlertsLocalRepository;
    private readonly ICacheRequestsLocalRepository _cacheRequestsLocalRepository;
    private readonly RestClient _client;
    private readonly IMapper _mapper;

    public BrandAlertRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
        _brandAlertsLocalRepository = DependencyService.Get<IBrandAlertsLocalRepository>();
        _mapper = DependencyService.Get<IMapper>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler())
        {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")
        };
        _client = new RestClient(httpClient);
        _client.AddDefaultHeader("Cookie", Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result);
    }

    public event ConnectionErrorHandler BadConnectEvent;

    public async Task<IList<BrandAlertRepositoryModel>> GetBrandAlerts(int brandId)
    {
        var json = JsonSerializer.Serialize(new {brandid = brandId, method = "GetAlertByBrandId"});

        if (await _cacheRequestsLocalRepository.Exists("be/brands/get_brand_alert", json))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetCacheRecords(x =>
                    x.RequestName == "be/brands/get_brand_alert" &&
                    x.RequestProperties == json &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _brandAlertsLocalRepository
                .GetItems(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<BrandAlertRepositoryModel>>(responseCache);
        }

        var request = new RestRequest("be/brands/get_brand_alert", Method.Post);

        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() {Success = false, StatusCode = (int) response.StatusCode});
            return new List<BrandAlertRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<BrandAlertRepositoryModel>>(response.Content) ??
                   new List<BrandAlertRepositoryModel>();

        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _brandAlertsLocalRepository.AddItem(_mapper.Map<BrandAlertLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.AddCacheRecord(new CacheRequestsLocalDatabaseModel
        {
            RequestName = "be/brands/get_brand_alert",
            RequestProperties = json,
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(5)
        });

        return list;
    }
}
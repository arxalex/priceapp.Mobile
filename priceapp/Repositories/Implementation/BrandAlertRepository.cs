using System.Net;
using System.Text.Json;
using AutoMapper;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.LocalDatabase.Models;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.Utils;
using RestSharp;

namespace priceapp.Repositories.Implementation;

public class BrandAlertRepository : IBrandAlertRepository
{
    private readonly IBrandAlertsLocalRepository _brandAlertsLocalRepository;
    private readonly ICacheRequestsLocalRepository _cacheRequestsLocalRepository;
    private readonly RestClient _client;
    private readonly IMapper _mapper;

    public BrandAlertRepository(
        IBrandAlertsLocalRepository brandAlertsLocalRepository, 
        ICacheRequestsLocalRepository cacheRequestsLocalRepository,
        IMapper mapper,
        HttpClient http
        ) {
        _brandAlertsLocalRepository = brandAlertsLocalRepository;
        _cacheRequestsLocalRepository = cacheRequestsLocalRepository;
        _mapper = mapper;
        _client = ConnectionUtil.GetRestClient(http);
    }

    public event ConnectionErrorHandler? BadConnectEvent;

    public async Task<IList<BrandAlertRepositoryModel>> GetBrandAlerts(int brandId)
    {
        var requestUrl = $"Brands/{brandId}/alerts";

        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, ""))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == "" &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _brandAlertsLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<BrandAlertRepositoryModel>>(responseCache);
        }

        var request = new RestRequest(requestUrl);

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs {Success = false, StatusCode = (int) response.StatusCode});
            return new List<BrandAlertRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<BrandAlertRepositoryModel>>(response.Content) ??
                   new List<BrandAlertRepositoryModel>();

        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _brandAlertsLocalRepository.InsertAsync(_mapper.Map<BrandAlertLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.InsertAsync(new CacheRequestsLocalDatabaseModel
        {
            RequestName = requestUrl,
            RequestProperties = "",
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(5)
        });

        return list;
    }
}
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

[assembly: Dependency(typeof(CategoryRepository))]
namespace priceapp.Repositories.Implementation;

public class CategoryRepository : ICategoryRepository
{
    public event ConnectionErrorHandler BadConnectEvent;
    private readonly RestClient _client;
    private readonly ICacheRequestsLocalRepository _cacheRequestsLocalRepository;
    private readonly ICategoriesLocalRepository _categoriesLocalRepository;
    private readonly IMapper _mapper;
    public CategoryRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
        _categoriesLocalRepository = DependencyService.Get<ICategoriesLocalRepository>();
        _mapper = DependencyService.Get<IMapper>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler()) {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")  
        };
        _client = new RestClient(httpClient);
        _client.AddDefaultHeader("Cookie", Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result);
    }
    public async Task<IList<CategoryRepositoryModel>> GetCategories(int? parent = null)
    {
        var json = JsonSerializer.Serialize(new { source = 0, parent });
        
        if (await _cacheRequestsLocalRepository.Exists("be/categories/get_categories", json))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetCacheRecords(x =>
                    x.RequestName == "be/categories/get_categories" &&
                    x.RequestProperties == json &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _categoriesLocalRepository
                .GetCategories(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<CategoryRepositoryModel>>(responseCache);
        }
        
        var request = new RestRequest("be/categories/get_categories", Method.Post);
        
        request.AddHeader("Content-Type", "application/json");
        request.AddBody(json, "application/json");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs(){Success = false, StatusCode = (int) response.StatusCode});
            return new List<CategoryRepositoryModel>();
        }

        var list = JsonSerializer.Deserialize<List<CategoryRepositoryModel>>(response.Content) ?? new List<CategoryRepositoryModel>();
        list.RemoveAll(x => x.id == 0);
        
        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _categoriesLocalRepository.AddCategory(_mapper.Map<CategoryLocalDatabaseModel>(item)));
        }

        await _cacheRequestsLocalRepository.AddCacheRecord(new CacheRequestsLocalDatabaseModel
        {
            RequestName = "be/categories/get_categories",
            RequestProperties = json,
            ResponseItemIds = JsonSerializer.Serialize(recordIds.ToArray()),
            Expires = DateTime.Now + TimeSpan.FromHours(480)
        });
        
        return list;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
        _categoriesLocalRepository = DependencyService.Get<ICategoriesLocalRepository>();
        _mapper = DependencyService.Get<IMapper>();
        _client = ConnectionUtil.GetRestClient();
    }

    public async Task<IList<CategoryRepositoryModel>> GetCategories(int? parent = null)
    {
        var requestUrl = parent == null ? "Categories/base" : $"Categories/{parent}/child";

        if (await _cacheRequestsLocalRepository.ExistsAsync(requestUrl, ""))
        {
            var responseCacheIds = JsonSerializer.Deserialize<int[]>((await _cacheRequestsLocalRepository
                .GetAsync(x =>
                    x.RequestName == requestUrl &&
                    x.RequestProperties == "" &&
                    x.Expires > DateTime.Now
                )).First().ResponseItemIds);

            var responseCache = await _categoriesLocalRepository
                .GetAsync(x => responseCacheIds.Contains(x.RecordId));

            return _mapper.Map<IList<CategoryRepositoryModel>>(responseCache);
        }

        var request = new RestRequest(requestUrl);

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            BadConnectEvent?.Invoke(this,
                new ConnectionErrorArgs() { Success = false, StatusCode = (int)response.StatusCode });
            return new List<CategoryRepositoryModel>();
        }
        var list = JsonSerializer.Deserialize<List<CategoryRepositoryModel>>(response.Content) ??
                   new List<CategoryRepositoryModel>();
        list.RemoveAll(x => x.id == 0);

        var recordIds = new List<int>();
        foreach (var item in list)
        {
            recordIds.Add(await _categoriesLocalRepository.InsertAsync(_mapper.Map<CategoryLocalDatabaseModel>(item)));
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
}
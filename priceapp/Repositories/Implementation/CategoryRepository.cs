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

[assembly: Dependency(typeof(CategoryRepository))]
namespace priceapp.Repositories.Implementation;

public class CategoryRepository : ICategoryRepository
{
    public event ConnectionErrorHandler BadConnectEvent;
    private readonly RestClient _client;
    public CategoryRepository()
    {
        var priceAppWebAccess = DependencyService.Get<IPriceAppWebAccess>();
        var httpClient = new HttpClient(priceAppWebAccess.GetHttpClientHandler()) {
            BaseAddress = new Uri("https://priceapp.arxalex.co/")  
        };
        _client = new RestClient(httpClient);
        _client.AddDefaultHeader("Cookie", Xamarin.Essentials.SecureStorage.GetAsync("cookie").Result);
    }
    public async Task<IList<CategoryRepositoryModel>> GetCategories(int? parent = null)
    {
        var request = new RestRequest("be/categories/get_categories", Method.Post);

        var json = JsonSerializer.Serialize(new { source = 0, parent });
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
        return list;
    }
}
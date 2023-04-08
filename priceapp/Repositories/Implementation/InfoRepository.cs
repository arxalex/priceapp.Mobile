using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.Utils;
using RestSharp;
using Xamarin.Forms;

[assembly: Dependency(typeof(InfoRepository))]

namespace priceapp.Repositories.Implementation;

public class InfoRepository : IInfoRepository
{
    private readonly RestClient _client;

    public InfoRepository()
    {
        _client = ConnectionUtil.GetRestClient();
    }
    

    public async Task<InfoRepositoryModel> GetInfo()
    {
        var request = new RestRequest("Info");

        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<InfoRepositoryModel>(response.Content);
    }
}
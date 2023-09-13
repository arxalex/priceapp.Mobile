using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Repositories.Implementation;
using priceapp.Repositories.Interfaces;
using priceapp.Repositories.Models;
using priceapp.Utils;
using RestSharp;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(InfoRepository))]

namespace priceapp.Repositories.Implementation;

public class InfoRepository : IInfoRepository
{
    private readonly RestClient _client = ConnectionUtil.GetRestClient();
    public event ConnectionErrorHandler BadConnectEvent;

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

    public async Task<bool?> IsAppNeedUpdate()
    {
        var request = new RestRequest("Info/check-update");
        request.AddQueryParameter("version", VersionTracking.CurrentVersion);
        
        var response = await _client.ExecuteAsync(request);
        if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        {
            ErrorResponseModel result = null;
            if (response.Content != null)
            {
                result = JsonSerializer.Deserialize<ErrorResponseModel>(response.Content);
            }

            BadConnectEvent?.Invoke(this, new ConnectionErrorArgs()
            {
                Message = result != null ? (result.message.Length > 0 ? result.message : "Не знайдено") : "Не знайдено",
                StatusCode = (int)response.StatusCode,
                Success = false
            });
            return null;
        }

        return JsonSerializer.Deserialize<bool>(response.Content);
    }
}
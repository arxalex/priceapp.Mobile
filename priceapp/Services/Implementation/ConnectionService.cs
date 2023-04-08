using System.Threading.Tasks;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(ConnectionService))]

namespace priceapp.Services.Implementation;

public class ConnectionService : IConnectionService
{
    private readonly IInfoRepository _infoRepository;
    
    public ConnectionService()
    {
        _infoRepository = DependencyService.Get<IInfoRepository>();
    }

    public async Task<bool> IsConnectedAsync()
    {
        return await _infoRepository.GetInfo() != null;
    }

    public async Task<bool> IsAppNeedsUpdateAsync()
    {
        return false;
        // var request = new RestRequest("be/check_version", Method.Post);
        //
        // var json = JsonSerializer.Serialize(new {installed = VersionTracking.CurrentVersion});
        //
        // request.AddHeader("Content-Type", "application/json");
        // request.AddBody(json, "application/json");
        //
        // var response = _client.ExecuteAsync(request).Result;
        //
        // if (response.StatusCode != HttpStatusCode.OK || response.Content == null)
        // {
        //     return false;
        // }
        //
        // var result = JsonSerializer.Deserialize<bool>(response.Content, new JsonSerializerOptions());
        //
        // return !result;
    }
}
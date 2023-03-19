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
    
    public bool IsConnected()
    {
        return _infoRepository.GetInfo().Result != null;
    }
    
    public async Task<bool> IsConnectedAsync()
    {
        return await _infoRepository.GetInfo() != null;
    }
}
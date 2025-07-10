using priceapp.Events.Delegates;
using priceapp.Events.Models;
using priceapp.Repositories.Interfaces;
using priceapp.Services.Interfaces;

namespace priceapp.Services.Implementation;

public class ConnectionService : IConnectionService
{
    private readonly IInfoRepository _infoRepository;
    public event ConnectionErrorHandler? BadConnectEvent;

    public ConnectionService(IInfoRepository infoRepository)
    {
        _infoRepository = infoRepository;
        _infoRepository.BadConnectEvent += InfoRepositoryOnBadConnectEvent;
    }

    private void InfoRepositoryOnBadConnectEvent(object sender, ConnectionErrorArgs args)
    {
        BadConnectEvent?.Invoke(this, args);
    }

    public async Task<bool> IsConnectedAsync()
    {
        return await _infoRepository.GetInfo() != null;
    }

    public async Task<bool?> IsAppNeedsUpdateAsync()
    {
        return await _infoRepository.IsAppNeedUpdate();
    }
}
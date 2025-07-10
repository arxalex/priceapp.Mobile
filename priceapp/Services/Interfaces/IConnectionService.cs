using priceapp.Events.Delegates;

namespace priceapp.Services.Interfaces;

public interface IConnectionService
{
    Task<bool> IsConnectedAsync();
    Task<bool?> IsAppNeedsUpdateAsync();
    event ConnectionErrorHandler BadConnectEvent;
}
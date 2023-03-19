using System.Threading.Tasks;

namespace priceapp.Services.Interfaces;

public interface IConnectionService
{
    bool IsConnected();
    Task<bool> IsConnectedAsync();
}
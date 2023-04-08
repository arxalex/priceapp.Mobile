using System.Threading.Tasks;

namespace priceapp.Services.Interfaces;

public interface IConnectionService
{
    Task<bool> IsConnectedAsync();
    Task<bool> IsAppNeedsUpdateAsync();
}
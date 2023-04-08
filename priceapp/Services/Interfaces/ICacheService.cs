using System.Threading.Tasks;

namespace priceapp.Services.Interfaces;

public interface ICacheService
{
    Task ClearAsync();
}
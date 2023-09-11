using System.Threading.Tasks;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CacheService))]

namespace priceapp.Services.Implementation;

public class CacheService : ICacheService
{
    private readonly IBrandAlertsLocalRepository _brandAlertsLocalRepository = DependencyService.Get<IBrandAlertsLocalRepository>();
    private readonly ICacheRequestsLocalRepository _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
    private readonly ICategoriesLocalRepository _categoriesLocalRepository = DependencyService.Get<ICategoriesLocalRepository>();
    private readonly IFilialsLocalRepository _filialsLocalRepository = DependencyService.Get<IFilialsLocalRepository>();
    private readonly IItemsLocalRepository _itemsLocalRepository = DependencyService.Get<IItemsLocalRepository>();
    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();
    private readonly IShopsLocalRepository _shopsLocalRepository = DependencyService.Get<IShopsLocalRepository>();

    public async Task ClearAsync()
    {
        await _cacheRequestsLocalRepository.DeleteAllAsync();
        await _itemsToBuyLocalRepository.DeleteAllAsync();
        await _itemsLocalRepository.DeleteAllAsync();
        await _categoriesLocalRepository.DeleteAllAsync();
        await _filialsLocalRepository.DeleteAllAsync();
        await _shopsLocalRepository.DeleteAllAsync();
        await _brandAlertsLocalRepository.DeleteAllAsync();
    }
}
using System.Threading.Tasks;
using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Services.Implementation;
using priceapp.Services.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(CacheService))]

namespace priceapp.Services.Implementation;

public class CacheService : ICacheService
{
    private readonly IBrandAlertsLocalRepository _brandAlertsLocalRepository;
    private readonly ICacheRequestsLocalRepository _cacheRequestsLocalRepository;
    private readonly ICategoriesLocalRepository _categoriesLocalRepository;
    private readonly IFilialsLocalRepository _filialsLocalRepository;
    private readonly IItemsLocalRepository _itemsLocalRepository;
    private readonly IItemsToBuyLocalRepository _itemsToBuyLocalRepository;
    private readonly IShopsLocalRepository _shopsLocalRepository;

    public CacheService()
    {
        _brandAlertsLocalRepository = DependencyService.Get<IBrandAlertsLocalRepository>();
        _cacheRequestsLocalRepository = DependencyService.Get<ICacheRequestsLocalRepository>();
        _categoriesLocalRepository = DependencyService.Get<ICategoriesLocalRepository>();
        _filialsLocalRepository = DependencyService.Get<IFilialsLocalRepository>();
        _itemsLocalRepository = DependencyService.Get<IItemsLocalRepository>();
        _itemsToBuyLocalRepository = DependencyService.Get<IItemsToBuyLocalRepository>();
        _shopsLocalRepository = DependencyService.Get<IShopsLocalRepository>();
    }

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
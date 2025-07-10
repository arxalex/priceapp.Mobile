using priceapp.LocalDatabase.Repositories.Interfaces;
using priceapp.Services.Interfaces;

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
    private readonly ILocationService _locationService;

    public CacheService(
        IBrandAlertsLocalRepository brandAlertsLocalRepository, 
        ICacheRequestsLocalRepository cacheRequestsLocalRepository, 
        ICategoriesLocalRepository categoriesLocalRepository, 
        IFilialsLocalRepository filialsLocalRepository, 
        IItemsLocalRepository itemsLocalRepository, 
        IItemsToBuyLocalRepository itemsToBuyLocalRepository,
        IShopsLocalRepository shopsLocalRepository,
        ILocationService locationService)
    {
        _brandAlertsLocalRepository = brandAlertsLocalRepository;
        _cacheRequestsLocalRepository = cacheRequestsLocalRepository;
        _categoriesLocalRepository = categoriesLocalRepository;
        _filialsLocalRepository = filialsLocalRepository;
        _itemsLocalRepository = itemsLocalRepository;
        _itemsToBuyLocalRepository = itemsToBuyLocalRepository;
        _shopsLocalRepository = shopsLocalRepository;
        _locationService = locationService;
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
        _locationService.ClearCustomLocationData();
    }
}
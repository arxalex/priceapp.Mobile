using priceapp.Events.Delegates;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IBrandAlertRepository
{
    Task<IList<BrandAlertRepositoryModel>> GetBrandAlerts(int brandId);
    event ConnectionErrorHandler BadConnectEvent;
}
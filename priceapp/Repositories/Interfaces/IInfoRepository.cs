using System.Threading.Tasks;
using priceapp.Repositories.Models;

namespace priceapp.Repositories.Interfaces;

public interface IInfoRepository
{
    Task<InfoRepositoryModel> GetInfo();
}
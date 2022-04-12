using AutoMapper;
using priceapp.Models;
using priceapp.Repositories.Models;

namespace priceapp.Utils;

public static class MapperUtil
{
    public static IMapper CreateMapper()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Category, CategoryRepositoryModel>();
            cfg.CreateMap<CategoryRepositoryModel, Category>(); 
            cfg.CreateMap<ItemRepositoryModel, Item>(); 
            cfg.CreateMap<Item, ItemRepositoryModel>();
        });

        return mapperConfiguration.CreateMapper();
    }
}
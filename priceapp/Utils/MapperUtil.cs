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
            cfg.CreateMap<PriceAndFilialRepositoryModel, ItemPriceInfo>()
                .BeforeMap((source, destination) =>
                {
                    destination.ItemId = source.itemId;
                    destination.Price = source.price;
                    destination.Quantity = source.quantity;
                    var shop = new Shop
                    {
                        Id = source.shopId,
                        Label = source.shopLabel,
                        Icon = source.shopIcon
                    };
                    destination.Shop = shop;
                    destination.Filial = new Filial
                    {
                        Id = source.filialId,
                        City = source.filialCity,
                        House = source.filialHouse,
                        Street = source.filialStreet,
                        XCord = source.xCord,
                        YCord = source.yCord,
                        Shop = shop
                    };
                });
            cfg.CreateMap<ItemPriceInfo, PriceAndFilialRepositoryModel>()
                .BeforeMap((source, destination) =>
                {
                    destination.itemId = source.ItemId;
                    destination.price = source.Price;
                    destination.quantity = source.Quantity;
                    destination.shopId = source.Shop.Id;
                    destination.shopLabel = source.Shop.Label;
                    destination.shopIcon = source.Shop.Icon;
                    destination.filialId = source.Filial.Id;
                    destination.filialCity = source.Filial.City;
                    destination.filialStreet = source.Filial.Street;
                    destination.filialHouse = source.Filial.House;
                    destination.xCord = source.Filial.XCord;
                    destination.yCord = source.Filial.YCord;
                });
        });

        return mapperConfiguration.CreateMapper();
    }
}
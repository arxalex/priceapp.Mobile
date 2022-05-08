using System.Text.Json;
using AutoMapper;
using priceapp.Models;
using priceapp.Repositories.Models;
using priceapp.LocalDatabase.Models;

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
            cfg.CreateMap<ItemToBuyLocalDatabaseModel, ItemToBuy>().BeforeMap((s, d) =>
            {
                d.Filial = s.FilialId != null ? new Filial() {Id = s.FilialId ?? 0} : null;
                d.Item = new Item(){Id = s.ItemId};
            });
            cfg.CreateMap<ItemToBuy, ItemToBuyLocalDatabaseModel>().BeforeMap((s, d) =>
            {
                d.FilialId = s.Filial?.Id;
                d.ItemId = s.Item.Id;
            });
            cfg.CreateMap<ItemLocalDatabaseModel, ItemRepositoryModel>().BeforeMap((s, d) =>
            {
                d.additional = JsonSerializer.Deserialize<object>(s.Additional);
                d.consist = JsonSerializer.Deserialize<object>(s.Additional);
            });
            cfg.CreateMap<ItemRepositoryModel, ItemLocalDatabaseModel>().BeforeMap((s, d) =>
            {
                d.Additional = JsonSerializer.Serialize(s.additional);
                d.Consist = JsonSerializer.Serialize(s.consist);
            });
            cfg.CreateMap<CategoryRepositoryModel, CategoryLocalDatabaseModel>();
            cfg.CreateMap<CategoryLocalDatabaseModel, CategoryRepositoryModel>();

        });

        return mapperConfiguration.CreateMapper();
    }
}
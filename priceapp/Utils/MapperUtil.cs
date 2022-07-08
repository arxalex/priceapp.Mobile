using System.Collections.ObjectModel;
using System.Text.Json;
using AutoMapper;
using priceapp.LocalDatabase.Models;
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
            cfg.CreateMap<Shop, ShopRepositoryModel>();
            cfg.CreateMap<ShopRepositoryModel, Shop>();
            cfg.CreateMap<FilialRepositoryModel, Filial>().BeforeMap((s, d) => { d.Shop = new Shop {Id = s.shopid}; });
            cfg.CreateMap<Filial, FilialRepositoryModel>().BeforeMap((s, d) => { d.shopid = s.Shop.Id; });

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
                d.Item = new Item() {Id = s.ItemId};
            });
            cfg.CreateMap<ItemToBuy, ItemToBuyLocalDatabaseModel>().BeforeMap((s, d) =>
            {
                d.FilialId = s.Filial?.Id;
                d.ItemId = s.Item.Id;
            });
            cfg.CreateMap<ItemLocalDatabaseModel, ItemRepositoryModel>().BeforeMap((s, d) =>
            {
                d.additional = s.Additional != null ? JsonSerializer.Deserialize<object>(s.Additional) : null;
                d.consist = s.Consist != null ? JsonSerializer.Deserialize<int[]>(s.Consist) : null;
            });
            cfg.CreateMap<ItemRepositoryModel, ItemLocalDatabaseModel>().BeforeMap((s, d) =>
            {
                d.Additional = JsonSerializer.Serialize(s.additional);
                d.Consist = JsonSerializer.Serialize(s.consist);
            });
            cfg.CreateMap<ItemLocalDatabaseModel, Item>().BeforeMap((s, d) =>
            {
                d.Additional = s.Additional != null ? JsonSerializer.Deserialize<object>(s.Additional) : null;
                d.Consist = s.Consist != null ? JsonSerializer.Deserialize<ObservableCollection<int>>(s.Consist) : null;
            });
            cfg.CreateMap<Item, ItemLocalDatabaseModel>().BeforeMap((s, d) =>
            {
                d.Additional = JsonSerializer.Serialize(s.Additional);
                d.Consist = JsonSerializer.Serialize(s.Consist);
            });
            cfg.CreateMap<CategoryRepositoryModel, CategoryLocalDatabaseModel>();
            cfg.CreateMap<CategoryLocalDatabaseModel, CategoryRepositoryModel>();
            cfg.CreateMap<ShopRepositoryModel, ShopLocalDatabaseModel>();
            cfg.CreateMap<ShopLocalDatabaseModel, ShopRepositoryModel>();
            cfg.CreateMap<FilialLocalDatabaseModel, FilialRepositoryModel>();
            cfg.CreateMap<FilialRepositoryModel, FilialLocalDatabaseModel>();
            cfg.CreateMap<FilialLocalDatabaseModel, Filial>()
                .BeforeMap((s, d) => { d.Shop = new Shop {Id = s.ShopId}; });
            cfg.CreateMap<Filial, FilialLocalDatabaseModel>().BeforeMap((s, d) => { d.ShopId = s.Shop.Id; });
            cfg.CreateMap<ItemToBuy, ItemToBuyRepositoryModel>().BeforeMap((s, d) =>
            {
                d.itemId = s.Item.Id;
                d.filialId = s.Filial?.Id;
            });
            cfg.CreateMap<BrandAlertRepositoryModel, BrandAlert>();
            cfg.CreateMap<BrandAlert, BrandAlertRepositoryModel>();
            cfg.CreateMap<BrandAlertLocalDatabaseModel, BrandAlertRepositoryModel>();
            cfg.CreateMap<BrandAlertRepositoryModel, BrandAlertLocalDatabaseModel>();
            cfg.CreateMap<BrandAlertLocalDatabaseModel, BrandAlert>();
            cfg.CreateMap<BrandAlert, BrandAlertLocalDatabaseModel>();
        });

        return mapperConfiguration.CreateMapper();
    }
}
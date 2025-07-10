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
            /* Base - Repository */
            cfg.CreateMap<BrandAlert, BrandAlertRepositoryModel>().ReverseMap();
            cfg.CreateMap<Category, CategoryRepositoryModel>().ReverseMap();
            cfg.CreateMap<Filial, FilialRepositoryModel>()
                .BeforeMap((s, d) => { d.shopId = s.Shop.Id; });
            cfg.CreateMap<FilialRepositoryModel, Filial>()
                .BeforeMap((s, d) => { d.Shop = new Shop { Id = s.shopId }; });
            cfg.CreateMap<Item, ItemRepositoryModel>().ReverseMap();
            cfg.CreateMap<ItemToBuy, ShoppingListRepositoryModel>()
                .BeforeMap((s, d) =>
                {
                    d.itemId = s.Item.Id;
                    d.filialId = s.Filial?.Id;
                });
            cfg.CreateMap<ShoppingListRepositoryModel, ItemToBuy>()
                .BeforeMap((s, d) =>
                {
                    d.Filial = s.filialId != null ? new Filial { Id = s.filialId ?? 0 } : null;
                    d.Item = new Item { Id = s.itemId };
                });
            cfg.CreateMap<PriceModel, PriceRepositoryModel>().ReverseMap();
            cfg.CreateMap<Shop, ShopRepositoryModel>().ReverseMap();

            /* Repository - LocalDatabase */
            cfg.CreateMap<BrandAlertRepositoryModel, BrandAlertLocalDatabaseModel>().ReverseMap();
            cfg.CreateMap<CategoryRepositoryModel, CategoryLocalDatabaseModel>().ReverseMap();
            cfg.CreateMap<FilialRepositoryModel, FilialLocalDatabaseModel>().ReverseMap();
            cfg.CreateMap<ItemRepositoryModel, ItemLocalDatabaseModel>()
                .AfterMap((s, d) =>
                {
                    d.Additional = JsonSerializer.Serialize(s.additional);
                    d.Consist = JsonSerializer.Serialize(s.consist);
                });
            cfg.CreateMap<ItemLocalDatabaseModel, ItemRepositoryModel>()
                .BeforeMap((s, d) =>
                {
                    d.additional = JsonSerializer.Deserialize<object>(s.Additional);
                    d.consist = JsonSerializer.Deserialize<List<int>>(s.Consist);
                })
                .ForMember(d => d.consist, opt => opt.Ignore())
                .ForMember(d => d.additional, opt => opt.Ignore());
            cfg.CreateMap<ShopRepositoryModel, ShopLocalDatabaseModel>().ReverseMap();

            /* LocalDatabase - Base */
            cfg.CreateMap<BrandAlertLocalDatabaseModel, BrandAlert>().ReverseMap();
            cfg.CreateMap<FilialLocalDatabaseModel, Filial>()
                .BeforeMap((s, d) => { d.Shop = new Shop { Id = s.ShopId }; });
            cfg.CreateMap<Filial, FilialLocalDatabaseModel>()
                .BeforeMap((s, d) => { d.ShopId = s.Shop.Id; });
            cfg.CreateMap<ItemLocalDatabaseModel, Item>()
                .BeforeMap((s, d) =>
                {
                    d.Additional = JsonSerializer.Deserialize<object>(s.Additional);
                    d.Consist = JsonSerializer.Deserialize<ObservableCollection<int>>(s.Consist);
                })
                .ForMember(d => d.Additional, opt => opt.Ignore())
                .ForMember(d => d.Consist, opt => opt.Ignore());
            cfg.CreateMap<Item, ItemLocalDatabaseModel>()
                .AfterMap((s, d) =>
                {
                    d.Additional = JsonSerializer.Serialize(s.Additional);
                    d.Consist = JsonSerializer.Serialize(s.Consist);
                });
            cfg.CreateMap<ItemToBuyLocalDatabaseModel, ItemToBuy>()
                .BeforeMap((s, d) =>
                {
                    d.Filial = s.FilialId != null ? new Filial { Id = s.FilialId ?? 0 } : null;
                    d.Item = new Item { Id = s.ItemId };
                });
            cfg.CreateMap<ItemToBuy, ItemToBuyLocalDatabaseModel>()
                .BeforeMap((s, d) =>
                {
                    d.FilialId = s.Filial?.Id;
                    d.ItemId = s.Item.Id;
                });
        });

        return mapperConfiguration.CreateMapper();
    }
}
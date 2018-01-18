using AutoMapper;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;

namespace CMSCore.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>().ConstructUsing(m => new ProductCategory(m.Name, m.Description, m.ParentId, m.HomeOrder, m.Image, m.HomeFlag, m.LevelCate, m.SortOrder, m.Status, m.SeoPageTitle, m.SeoAlias, m.SeoKeywords, m.SeoDescription));

            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
                    c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
                    c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));
        }
    }
}
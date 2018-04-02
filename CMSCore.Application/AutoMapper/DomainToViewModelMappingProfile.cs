using AutoMapper;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;

namespace CMSCore.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            #region Product

            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<ProductQuantity, ProductQuantityViewModel>();
            CreateMap<ProductImage, ProductImageViewModel>();

            #endregion Product

            #region Blog

            CreateMap<Blog, BlogViewModel>();
            CreateMap<BlogTag, BlogTagViewModel>();

            #endregion Blog

            #region Common

            CreateMap<Footer, FooterViewModel>();
            CreateMap<SystemConfig, SystemConfigViewModel>();
            CreateMap<Slide, SlideViewModel>();

            #endregion Common

            #region System

            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();

            #endregion System
        }
    }
}
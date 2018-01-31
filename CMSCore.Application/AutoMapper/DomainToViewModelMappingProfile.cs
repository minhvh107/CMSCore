using AutoMapper;
using CMSCore.Application.ViewModels.System;
using CMSCore.Application.ViewModels.Product;
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

            #endregion

            #region System
            CreateMap<Function, FunctionViewModel>();
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();

            #endregion
        }
    }
}
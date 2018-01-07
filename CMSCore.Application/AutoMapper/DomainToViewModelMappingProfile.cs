using AutoMapper;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;

namespace CMSCore.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<ProductCategory, ProductCategoryViewModel>();
        }
    }
}
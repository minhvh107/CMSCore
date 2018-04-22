using AutoMapper;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;
using System;

namespace CMSCore.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            #region Product
            CreateMap<ProductCategoryViewModel, ProductCategory>().ConstructUsing(m => new ProductCategory(m.Name, m.Description, m.ParentId, m.HomeOrder, m.Image, m.HomeFlag, m.LevelCate,
                m.Status, m.IsDelete, m.SortOrder,
                m.SeoPageTitle, m.SeoAlias, m.SeoKeywords, m.SeoDescription));

            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
                    c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit,
                    c.Status, c.IsDelete, c.SortOrder,
                    c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));

            CreateMap<BillViewModel,Bill>()
                .ConstructUsing(c => new Bill(c.Id, c.CustomerName,c.CustomerAddress,c.CustomerMobile,c.CustomerMessage,c.BillStatus,c.PaymentMethod,c.Status,c.CustomerId,c.DateCreated));

            CreateMap<BillDetailViewModel, BillDetail>()
                .ConstructUsing(c => new BillDetail(c.Id, c.BillId, c.ProductId, c.Quantity, c.Price, c.ColorId, c.SizeId));
            #endregion

            #region System
            CreateMap<AppUserViewModel, AppUser>()
                .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.UserName,
                    c.Email, c.PhoneNumber, c.Avatar, c.Status));

            CreateMap<PermissionViewModel, Permission>()
                .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.ActionId));
            #endregion

            #region Common
            CreateMap<ContactViewModel, Contact>()
                .ConstructUsing(c => new Contact(c.Id, c.Name, c.Phone, c.Email, c.Website, c.Address, c.Other, c.Lng, c.Lat, c.Status));

            CreateMap<FeedbackViewModel, Feedback>()
                .ConstructUsing(c => new Feedback(c.Id, c.Name, c.Email, c.Message, c.Status));


            #endregion
        }
    }
}
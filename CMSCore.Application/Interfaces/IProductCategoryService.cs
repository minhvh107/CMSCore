﻿using System.Collections.Generic;
using CMSCore.Application.ViewModels;
using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.Interfaces
{
    public interface IProductCategoryService
    {
        ProductCategoryViewModel Create(ProductCategoryViewModel productCategoryVm);

        void Update(ProductCategoryViewModel productCategoryVm);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();

        PagedResult<ProductCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize);

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        ProductCategoryViewModel GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);
        void ReOrder(int sourceId, int targetId);

        List<ProductCategoryViewModel> GetHomeCategories(int top);

        void Save();
    }
}
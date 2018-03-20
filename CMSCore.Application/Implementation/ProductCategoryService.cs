using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;
using CMSCore.Data.Enums;
using CMSCore.Data.IRepositories;
using CMSCore.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;
using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategporyRepository, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategporyRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productCategoryVm;
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategoryRepository.Update(productCategory);
        }

        public void Delete(int id)
        {
            _productCategoryRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _productCategoryRepository.FindAll().OrderBy(m => m.LevelCate).ThenBy(m => m.ParentId).ThenBy(m=>m.SortOrder).ProjectTo<ProductCategoryViewModel>()
                .ToList();
        }

        public PagedResult<ProductCategoryViewModel> GetAllPaging(string keyword, int page, int pageSize)
        {
            var query = _productCategoryRepository.FindAll(m => m.IsDelete == false);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.Name.Contains(keyword));
            }
            var totalRow = query.Count();
            query = query.OrderByDescending(m => m.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.ProjectTo<ProductCategoryViewModel>().ToList();
            var paginationSet = new PagedResult<ProductCategoryViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _productCategoryRepository
                    .FindAll(m => m.Name.Contains(keyword) || m.Description.Contains(keyword)).OrderBy(m => m.ParentId)
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            }
            else
            {
                return _productCategoryRepository.FindAll().OrderBy(m => m.ParentId)
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            }
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.FindAll(m => m.Status == Status.Active && m.ParentId == parentId)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = _productCategoryRepository.FindById(sourceId);
            sourceCategory.ParentId = targetId;
            sourceCategory.LevelCate = _productCategoryRepository.FindById(targetId).LevelCate + 1;
            _productCategoryRepository.Update(sourceCategory);

            //Get all sibling
            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }

        public void ReOrder(int sourceId, int targetId)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            var tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;
            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategoryRepository.FindAll(m => m.HomeFlag == true, c => c.Products).OrderBy(m => m.HomeOrder).Take(top).ProjectTo<ProductCategoryViewModel>();
            var categories = query.ToList();
            //foreach (var category in categories)
            //{
            //}
            return categories;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

    }
}
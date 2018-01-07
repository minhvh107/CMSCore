using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Data.EF.Repositories;
using CMSCore.Data.Entities;
using CMSCore.Data.Enums;
using CMSCore.Infrastructure.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Application.Implementation
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly ProductCategporyRepository _productCategporyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(ProductCategporyRepository productCategporyRepository, IUnitOfWork unitOfWork)
        {
            _productCategporyRepository = productCategporyRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productCategoryVm)
        {
            var productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productCategoryVm);
            _productCategporyRepository.Add(productCategory);
            return productCategoryVm;
        }

        public void Update(ProductCategoryViewModel productCategoryVm)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            _productCategporyRepository.Remove(id);
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return _productCategporyRepository.FindAll().OrderBy(m => m.ParentId).ProjectTo<ProductCategoryViewModel>()
                .ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return _productCategporyRepository
                    .FindAll(m => m.Name.Contains(keyword) || m.Description.Contains(keyword)).OrderBy(m => m.ParentId)
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            }
            else
            {
                return _productCategporyRepository.FindAll().OrderBy(m => m.ParentId)
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            }
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategporyRepository.FindAll(m => m.Status == Status.Active && m.ParentId == parentId)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategporyRepository.FindById(id));
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            throw new System.NotImplementedException();
        }

        public void ReOrder(int sourceId, int targetId)
        {
            throw new System.NotImplementedException();
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategporyRepository.FindAll(m => m.HomeFlag == true, c => c.Products).OrderBy(m=>m.HomeOrder).Take(top).ProjectTo<ProductCategoryViewModel>();
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
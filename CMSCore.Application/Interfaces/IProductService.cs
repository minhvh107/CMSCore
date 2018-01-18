using CMSCore.Application.ViewModels;
using System.Collections.Generic;
using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.Interfaces
{
    public interface IProductService
    {
        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        ProductViewModel Add(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        void Save();
    }
}
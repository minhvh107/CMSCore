using CMSCore.Application.ViewModels;
using System.Collections.Generic;
using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.Interfaces
{
    public interface IProductService
    {
        ProductViewModel Create(ProductViewModel product);

        void Update(ProductViewModel product);

        void Delete(int id);

        ProductViewModel GetById(int id);

        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        void ImportExcel(string filePath, int categoryId);

        void Save();
    }
}
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

        List<ProductQuantityViewModel> GetQuantities(int productId);

        void CreateQuantities(int productId, List<ProductQuantityViewModel> quantity);

        List<ProductImageViewModel> GetImages(int productId);

        void CreateImages(int productId,string [] images);

        void Save();

        List<ProductViewModel> GetHotProduct(int top);

        List<ProductViewModel> GetLastest(int top);

        List<ProductViewModel> GetRelatedProducts(int id, int top);

        List<ProductViewModel> GetUpsellProducts(int top);

        List<TagViewModel> GetProductTags(int productId);
    }
}
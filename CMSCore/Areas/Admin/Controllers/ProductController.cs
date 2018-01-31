using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels.Product;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax Api

        /// <summary>
        /// Lấy tất cả danh sách sản phẩm
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productService.GetAll();
            return new OkObjectResult(model);
        }

        /// <summary>
        /// Lấy tất cả danh sách nhóm sản phẩm
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllCategory()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        /// <summary>
        /// Lấy danh sách sản phẩm theo phân trang
        /// </summary>
        /// <param name="categoryId">Id của nhóm sản phẩm</param>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng trên 1 trang</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        /// <summary>
        /// Lấy chi tiết 1 bản ghi theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);

            return new OkObjectResult(model);
        }

        /// <summary>
        /// Save and Edit sản phẩm
        /// </summary>
        /// <param name="productVm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
            if (productVm.Id == 0)
            {
                _productService.Add(productVm);
            }
            else
            {
                _productService.Update(productVm);
            }
            _productService.Save();
            return new OkObjectResult(productVm);
        }

        /// <summary>
        /// Xóa sản phẩm theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            _productService.Delete(id);
            _productService.Save();

            return new OkObjectResult(id);
        }

        #endregion Ajax Api
    }
}
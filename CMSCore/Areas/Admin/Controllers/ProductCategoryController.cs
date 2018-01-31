using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels.Product;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;

        public ProductCategoryController(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        /// <summary>
        /// Danh sách nhóm sản phẩm
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Lấy tất cả nhóm sản phẩm
        /// </summary>
        /// <returns></returns>
        public IActionResult GetAll()
        {
            var model = _productCategoryService.GetAll();
            return new ObjectResult(model);
        }

        /// <summary>
        /// Lấy dữ liệu theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productCategoryService.GetById(id);

            return new ObjectResult(model);
        }

        /// <summary>
        /// Update lại cấp cha
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="targetId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.UpdateParentId(sourceId, targetId, items);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        /// <summary>
        /// Update lại thứ tự
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ReOrder(int sourceId, int targetId)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }
            else
            {
                if (sourceId == targetId)
                {
                    return new BadRequestResult();
                }
                else
                {
                    _productCategoryService.ReOrder(sourceId, targetId);
                    _productCategoryService.Save();
                    return new OkResult();
                }
            }
        }

        /// <summary>
        /// Lưu dữ liệu
        /// </summary>
        /// <param name="productVm"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveEntity(ProductCategoryViewModel productVm)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
            if (productVm.ParentId != 0)
            {
                var categoryParent = _productCategoryService.GetById(productVm.ParentId);
                productVm.LevelCate = categoryParent.LevelCate + 1;
            }
            if (productVm.Id == 0)
            {
                _productCategoryService.Add(productVm);
            }
            else
            {
                _productCategoryService.Update(productVm);
            }
            _productCategoryService.Save();
            return new OkObjectResult(productVm);
        }

        /// <summary>
        /// Xóa nhóm sản phẩm theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return new BadRequestResult();
            }
            _productCategoryService.Delete(id);
            _productCategoryService.Save();
            return new OkObjectResult(id);
        }
    }
}
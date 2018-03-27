using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Services;
using CMSCore.Utilities.Constants;
using CMSCore.Utilities.Dtos;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        private readonly IProductCategoryService _productCategoryService;
        private readonly IViewRenderService _viewRenderService;

        public ProductCategoryController(IProductCategoryService productCategoryService,
            IViewRenderService viewRenderService)
        {
            _productCategoryService = productCategoryService;
            _viewRenderService = viewRenderService;
        }

        /// <summary>
        /// Danh sách nhóm sản phẩm
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            const string keyword = "";
            const int currentPage = 1;
            const int pageSize = 10;
            var lstObj = _productCategoryService.GetAllPaging(keyword, currentPage, pageSize);
            var model = new PageProductCategoryViewModel()
            {
                ListProductCategoryViewModels = lstObj.Results,
                PagedResult = lstObj
            };

            return View(model);
        }

        #region Tìm kiếm

        /// <summary>
        /// Hàm lọc dữ liệu
        /// </summary>
        /// <param name="filter">Dữ liệu lọc</param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Filter(PagedResultBase filter)
        {
            var data = _productCategoryService.GetAllPaging(filter.Keyword, filter.CurrentPage, filter.PageSize);
            var listContent = _viewRenderService.RenderToStringAsync("ProductCategory/_ListProductCategory", data.Results);
            var pagination = _viewRenderService.RenderToStringAsync("Common/_CommonPagination", data);
            return Json(new JsonResponse()
            {
                Success = true,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Message = Constants.GetDataSuccess,
                Data = listContent.Result,
                Pagination = pagination.Result
            });
        }

        #endregion Tìm kiếm

        #region Thêm sản phẩm

        /// <summary>
        /// Show popup thêm mới sản phẩm
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            #region Danh sách nhóm sản phẩm

            var lstProductCate = _productCategoryService.GetAll();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn cấp cha—"
                }
            };
            lstSelect.AddRange(lstProductCate.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));

            #endregion Danh sách nhóm sản phẩm

            var model = new ProductCategoryViewModel
            {
                IsEdit = false,
                ListProductCate = lstSelect
            };

            var content = _viewRenderService.RenderToStringAsync("ProductCategory/_AddEditModal", model);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Thêm sản phẩm

        #region Sửa sản phẩm

        /// <summary>
        /// Sửa thông tin sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = _productCategoryService.GetById(id);

            obj.IsEdit = true;
            #region Danh sách nhóm sản phẩm

            var lstProductCate = _productCategoryService.GetAll();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn cấp cha—"
                }
            };
            lstSelect.AddRange(lstProductCate.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));
            obj.ListProductCate = lstSelect;

            #endregion Danh sách nhóm sản phẩm

            var content = _viewRenderService.RenderToStringAsync("ProductCategory/_AddEditModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Sửa sản phẩm

        #region Xóa sản phẩm

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
                return Json(new JsonResponse
                {
                    Success = false,
                    Message = Constants.DeleteDataFailed,
                    StatusCode = Utilities.Constants.StatusCode.SaveDataError
                });
            }
            _productCategoryService.Delete(id);
            _productCategoryService.Save();

            return Json(new JsonResponse()
            {
                Success = true,
                Message = Constants.DeleteDataSuccess
            });
        }

        #endregion Xóa sản phẩm

        #region Xem thông tin

        /// <summary>
        /// Xem thông tin sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ViewItem(int id)
        {
            var obj = _productCategoryService.GetById(id);

            obj.IsEdit = false;
            obj.IsView = true;

            #region Danh sách nhóm sản phẩm

            var lstProductCate = _productCategoryService.GetAll();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn cấp cha—"
                }
            };
            lstSelect.AddRange(lstProductCate.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));
            obj.ListProductCate = lstSelect;

            #endregion Danh sách nhóm sản phẩm

            var content = _viewRenderService.RenderToStringAsync("ProductCategory/_AddEditModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result
            });
        }

        #endregion Xem thông tin

        #region Lưu sản phẩm

        /// <summary>
        /// Save and Edit sản phẩm
        /// </summary>
        /// <param name="productCategoryVm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveItem(ProductCategoryViewModel productCategoryVm)
        {
            if (ModelState.IsValid)
            {
                var errors = new Dictionary<string, string>();
                productCategoryVm.SeoAlias = TextHelper.ToUnsignString(productCategoryVm.Name);
                if (productCategoryVm.IsEdit == false)
                {
                    _productCategoryService.Create(productCategoryVm);
                }
                else
                {
                    _productCategoryService.Update(productCategoryVm);
                }
                _productCategoryService.Save();
                return Json(new JsonResponse()
                {
                    Success = true,
                    Message = Constants.SaveDataSuccess
                });
            }
            return Json(new JsonResponse
            {
                Success = false,
                Message = Constants.SaveDataFailed,
                Errors = ModelState.Where(modelState => modelState.Value.Errors.Count > 0).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).FirstOrDefault()
                )
            });
        }

        #endregion Lưu sản phẩm

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
    }
}
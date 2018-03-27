using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Services;
using CMSCore.Utilities.Constants;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace CMSCore.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        #region Init

        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;

        public ProductController(IProductService productService,
            IProductCategoryService productCategoryService,
            IHostingEnvironment hostingEnvironment,
            IViewRenderService viewRenderService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
        }

        #endregion Init

        #region Index

        public IActionResult Index(int? categoryId)
        {
            const string keyword = "";
            const int currentPage = 1;
            const int pageSize = 10;
            var lstObj = _productService.GetAllPaging(categoryId, keyword, currentPage, pageSize);

            var lstCate = _productCategoryService.GetAll();
            ViewData["lstCate"] = lstCate;

            var model = new PageProductViewModel()
            {
                ListProductViewModels = lstObj.Results,
                PagedResult = lstObj
            };

            return View(model);
        }

        #endregion Index

        #region Tìm kiếm

        /// <summary>
        /// Hàm lọc dữ liệu
        /// </summary>
        /// <param name="filter">Dữ liệu lọc</param>
        /// <param name="Keyword"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Filter(ProductFilterViewModel filter)
        {
            var data = _productService.GetAllPaging(filter.CategoryId, filter.Keyword, filter.CurrentPage,
                filter.PageSize);
            var listContent = _viewRenderService.RenderToStringAsync("Product/_ListProduct", data.Results);
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
                    Text = "—Chọn nhóm sản phẩm—"
                }
            };
            lstSelect.AddRange(lstProductCate.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));

            #endregion Danh sách nhóm sản phẩm

            var model = new ProductViewModel
            {
                IsEdit = false,
                ListProductCate = lstSelect
            };

            var content = _viewRenderService.RenderToStringAsync("Product/_AddEditModal", model);
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
            var obj = _productService.GetById(id);

            obj.IsEdit = true;

            #region Danh sách nhóm sản phẩm

            var lstProductCate = _productCategoryService.GetAll();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn nhóm sản phẩm—"
                }
            };
            lstSelect.AddRange(lstProductCate.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));
            obj.ListProductCate = lstSelect;

            #endregion Danh sách nhóm sản phẩm

            var content = _viewRenderService.RenderToStringAsync("Product/_AddEditModal", obj);
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
            _productService.Delete(id);
            _productService.Save();

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
            var obj = _productService.GetById(id);

            obj.IsEdit = false;
            obj.IsView = true;

            #region Danh sách nhóm sản phẩm

            var lstProductCate = _productCategoryService.GetAll();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn nhóm sản phẩm—"
                }
            };
            lstSelect.AddRange(lstProductCate.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));
            obj.ListProductCate = lstSelect;

            #endregion Danh sách nhóm sản phẩm

            var content = _viewRenderService.RenderToStringAsync("Product/_AddEditModal", obj);
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
        /// <param name="productVm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveItem(ProductViewModel productVm)
        {
            if (ModelState.IsValid)
            {
                var errors = new Dictionary<string, string>();
                productVm.SeoAlias = TextHelper.ToUnsignString(productVm.Name);
                if (productVm.IsEdit == false)
                {
                    _productService.Create(productVm);
                }
                else
                {
                    _productService.Update(productVm);
                }
                _productService.Save();
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

        #region Export sản phẩm

        /// <summary>
        /// Xuất danh sách sản phẩm
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ExportExcel(ProductFilterViewModel filter)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string directory = Path.Combine(sWebRootFolder, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string sFileName = $"Product_{DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            // var products = _productService.GetAll();
            var products = _productService.GetAllPaging(filter.CategoryId, filter.Keyword, filter.CurrentPage,
                filter.PageSize);

            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells["A1"].LoadFromCollection(products.Results, true, TableStyles.Light1);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = fileUrl
            });
        }

        #endregion Export sản phẩm

        #region Thêm nhiều sản phẩm
        [HttpGet]
        public ActionResult ImportExcel()
        {
            #region Danh sách nhóm sản phẩm

            var lstProductCate = _productCategoryService.GetAll();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn nhóm sản phẩm—"
                }
            };
            lstSelect.AddRange(lstProductCate.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));

            #endregion Danh sách nhóm sản phẩm

            var model = new ProductViewModel
            {
                IsEdit = false,
                ListProductCate = lstSelect
            };
            var content = _viewRenderService.RenderToStringAsync("Product/_ImportExcelModal", model);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        /// <summary>
        /// Thêm nhiều sản phẩm
        /// </summary>
        /// <param name="files"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int CategoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var filename = ContentDispositionHeaderValue
                                   .Parse(file.ContentDisposition)
                                   .FileName
                                   .Trim('"');

                string folder = _hostingEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                string filePath = Path.Combine(folder, filename);

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _productService.ImportExcel(filePath, CategoryId);
                _productService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        #endregion Thêm nhiều sản phẩm
    }
}
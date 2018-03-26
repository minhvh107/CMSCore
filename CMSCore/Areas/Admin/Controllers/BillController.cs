using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Enums;
using CMSCore.Services;
using CMSCore.Utilities.Constants;
using CMSCore.Utilities.Extensions;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        #region Default

        private readonly IBillService _billService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;
        private readonly IProductService _productService;

        public BillController(
            IBillService billService,
            IHostingEnvironment hostingEnvironment,
            IViewRenderService viewRenderService,
            IProductService productService
        )
        {
            _billService = billService;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
            _productService = productService;
        }

        #endregion Default

        #region Bill

        #region Index

        /// <summary>
        /// Danh sách hoá đơn
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            const string keyword = "";
            const int currentPage = 1;
            const int pageSize = 10;
            var lstObj = _billService.GetAllPaging("", "", keyword, currentPage, pageSize);
            var model = new PageBillViewModel()
            {
                PagedResult = lstObj,
                ListBillViewModels = lstObj.Results
            };
            return View(model);
        }

        #endregion Index

        #region Tìm kiếm

        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IActionResult Filter(BillFillterViewModel filter)
        {
            var data = _billService.GetAllPaging(filter.StartDate, filter.EndDate, filter.Keyword, filter.CurrentPage,
                filter.PageSize);
            var listContent = _viewRenderService.RenderToStringAsync("Bill/_ListBill", data.Results);
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

        #region Thêm

        public IActionResult Add()
        {
            var lstPaymentMethod = ListPaymentMethod();
            var lstBillStatuus = ListBillStatus();
            var model = new BillViewModel()
            {
                IsEdit = false,
                ListPaymentMethods = lstPaymentMethod,
                ListBillStatus = lstBillStatuus
            };

            var content = _viewRenderService.RenderToStringAsync("Bill/_AddEditModal", model);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Thêm

        #endregion Bill

        #region Bill Detail

        #region Thêm Bill Detail

        public IActionResult AddDetail()
        {
            var lstProducts = ListProducts();
            var lstColors = ListColors();
            var lstSizes = ListSizes();
            var model = new BillDetailViewModel()
            {
                IsEdit = false,
                ListProducts = lstProducts,
                ListColors = lstColors,
                ListSizes = lstSizes
            };

            var content = _viewRenderService.RenderToStringAsync("Bill/_AddEditDetailModal", model);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Thêm Bill Detail

        #endregion Bill Detail

        #region Init List

        #region Danh sách loại thanh toán

        /// <summary>
        /// Danh sách loại thanh toán
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListPaymentMethod()
        {
            var enumViewModels = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod)))
                .Select(c => new SelectListItem()
                {
                    Value = ((int)c).ToString(),
                    Text = c.GetDescription()
                }).ToList();
            return enumViewModels;
        }

        #endregion Danh sách loại thanh toán

        #region Danh sách trạng thái

        /// <summary>
        /// Danh sách trạng thái
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListBillStatus()
        {
            var enumViewModels = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
                .Select(c => new SelectListItem()
                {
                    Value = ((int)c).ToString(),
                    Text = c.GetDescription()
                }).ToList();
            return enumViewModels;
        }

        #endregion Danh sách trạng thái

        #region Danh sách color

        /// <summary>
        /// Danh sách trạng thái
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> ListColors()
        {
            var lstColor = _billService.GetColors();
            var result = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "",
                    Text = "—Chọn màu sắc—"
                }
            };
            result.AddRange(lstColor.Select(m => new SelectListItem()
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }).ToList());

            return result;
        }

        #endregion Danh sách color

        #region Danh sách size

        public List<SelectListItem> ListSizes()
        {
            var lstSize = _billService.GetSizes();
            var result = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "",
                    Text = "—Chọn Size—"
                }
            };
            result.AddRange(lstSize.Select(m => new SelectListItem()
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));
            return result;
        }

        #endregion Danh sách size

        #region Danh sách sản phẩm

        public List<SelectListItem> ListProducts()
        {
            var lstProduct = _productService.GetAll();
            var result = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Value = "",
                    Text = "—Chọn sản phẩm—"
                }
            };
            result.AddRange(lstProduct.Select(m => new SelectListItem()
            {
                Value = m.Id.ToString(),
                Text = m.Name
            }));
            return result;
        }

        #endregion Danh sách sản phẩm

        #endregion Init List
    }
}
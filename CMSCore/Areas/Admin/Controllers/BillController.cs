using System;
using System.Collections.Generic;
using System.Linq;
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Application.ViewModels.Common;
using CMSCore.Data.Enums;
using CMSCore.Services;
using CMSCore.Utilities.Constants;
using CMSCore.Utilities.Extensions;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSCore.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;

        public BillController(
            IBillService billService,
            IHostingEnvironment hostingEnvironment,
            IViewRenderService viewRenderService
            )
        {
            _billService = billService;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
        }

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


        #endregion

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
        #endregion

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
        #endregion
    }
}
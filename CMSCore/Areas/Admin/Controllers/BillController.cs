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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace CMSCore.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        #region Default

        private readonly IBillService _billService;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IViewRenderService _viewRenderService;
        private readonly IProductService _productService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;

        public BillController(
            IBillService billService,
            IHostingEnvironment hostingEnvironment,
            IViewRenderService viewRenderService,
            IProductService productService,
            IColorService colorService,
            ISizeService sizeService
        )
        {
            _billService = billService;
            _hostingEnvironment = hostingEnvironment;
            _viewRenderService = viewRenderService;
            _productService = productService;
            _colorService = colorService;
            _sizeService = sizeService;
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

        #region Sửa

        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="id">Mã</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = _billService.GetById(id);
            obj.IsEdit = true;
            obj.ListPaymentMethods = ListPaymentMethod();
            obj.ListBillStatus = ListBillStatus();
            var content = _viewRenderService.RenderToStringAsync("Bill/_AddEditModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Sửa

        #region Xóa

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
            _billService.Delete(id);
            _billService.Save();

            return Json(new JsonResponse()
            {
                Success = true,
                Message = Constants.DeleteDataSuccess
            });
        }

        #endregion Xóa

        #region Xem thông tin

        /// <summary>
        /// Xem thông tin sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewItem(int id)
        {
            var obj = _billService.GetById(id);

            obj.IsEdit = false;
            obj.IsView = true;
            obj.ListPaymentMethods = ListPaymentMethod();
            obj.ListBillStatus = ListBillStatus();

            var content = await _viewRenderService.RenderToStringAsync("Bill/_AddEditModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content
            });
        }

        #endregion Xem thông tin

        #region Lưu

        /// <summary>
        /// Save and Edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult SaveItem(BillViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.BillDetails = JsonConvert.DeserializeObject<List<BillDetailViewModel>>(model.JsonListBillDetails);
                if (model.IsEdit == false)
                {
                    _billService.Create(model);
                }
                else
                {
                    _billService.Update(model);
                }
                _billService.Save();
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

        #region Print Excel

        public ActionResult PrintExcel(int id)
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = $"Bill_{id}.xlsx";
            // Template File
            string templateDocument = Path.Combine(sWebRootFolder, "templates", "BillTemplate.xlsx");

            string url = $"{Request.Scheme}://{Request.Host}/{"export-files"}/{sFileName}";
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, "export-files", sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, "export-files", sFileName));
            }
            using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDocument))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // add a new worksheet to the empty workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["TEDUOrder"];
                    // Data Acces, load order header data.
                    var billDetail = _billService.GetById(id);

                    // Insert customer data into template
                    worksheet.Cells[4, 1].Value = "Customer Name: " + billDetail.CustomerName;
                    worksheet.Cells[5, 1].Value = "Address: " + billDetail.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "Phone: " + billDetail.CustomerMobile;
                    // Start Row for Detail Rows
                    int rowIndex = 9;

                    // load order details
                    var orderDetails = _billService.GetBillDetails(id);
                    int count = 1;
                    foreach (var orderDetail in orderDetails)
                    {
                        // Cell 1, Carton Count
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();
                        // Cell 2, Order Number (Outline around columns 2-7 make it look like 1 column)
                        worksheet.Cells[rowIndex, 2].Value = orderDetail.Product.Name;
                        // Cell 8, Weight in LBS (convert KG to LBS, and rounding to whole number)
                        worksheet.Cells[rowIndex, 3].Value = orderDetail.Quantity.ToString();

                        worksheet.Cells[rowIndex, 4].Value = orderDetail.Price.ToString("N0");
                        worksheet.Cells[rowIndex, 5].Value = (orderDetail.Price * orderDetail.Quantity).ToString("N0");
                        // Increment Row Counter
                        rowIndex++;
                        count++;
                    }
                    decimal total = (decimal)(orderDetails.Sum(x => x.Quantity * x.Price));
                    worksheet.Cells[24, 5].Value = total.ToString("N0");

                    var numberWord = "Total amount (by word): " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    var billDate = billDetail.DateCreated;
                    worksheet.Cells[28, 3].Value = billDate.Day + ", " + billDate.Month + ", " + billDate.Year;


                    package.SaveAs(file); //Save the workbook.
                }
            }
            return Json(new JsonResponse()
            {
                Success = true,
                Message = Constants.SaveDataSuccess,
                Data = url
            });
        }
        

        #endregion

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

        #region Sửa

        /// <summary>
        /// Sửa thông tin
        /// </summary>
        /// <param name="obj">Mã</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditDetail(BillDetailViewModel obj)
        {
            obj.IsEdit = true;
            obj.ListProducts = ListProducts();
            obj.ListColors = ListColors();
            obj.ListSizes = ListSizes();
            var content = _viewRenderService.RenderToStringAsync("Bill/_AddEditDetailModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Sửa

        #region Xem thông tin

        /// <summary>
        /// Xem thông tin
        /// </summary>
        /// <param name="obj">Mã </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ViewItemDetail(BillDetailViewModel obj)
        {
            obj.IsEdit = false;
            obj.IsView = true;
            obj.ListProducts = ListProducts();
            obj.ListColors = ListColors();
            obj.ListSizes = ListSizes();
            var content = await _viewRenderService.RenderToStringAsync("Bill/_AddEditDetailModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content
            });
        }

        #endregion Xem thông tin

        #region Lưu

        /// <summary>
        /// Save and Edit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveItemDetail(BillDetailViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Product = _productService.GetById(model.ProductId);
                model.Size = _sizeService.GetAll().FirstOrDefault(m => m.Id == model.SizeId);
                model.Color = _colorService.GetAll().FirstOrDefault(m => m.Id == model.ColorId);
                return Json(new JsonResponse()
                {
                    Success = true,
                    Message = Constants.SaveDataSuccess,
                    Data = await _viewRenderService.RenderToStringAsync("Bill/_ListBillDetail", model)
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
            var lstColor = _colorService.GetAll();
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
            var lstSize = _sizeService.GetAll();
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
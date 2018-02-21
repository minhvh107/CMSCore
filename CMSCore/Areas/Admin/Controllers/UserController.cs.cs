using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels.Default;
using CMSCore.Application.ViewModels.System;
using CMSCore.Authorization;
using CMSCore.Services;
using CMSCore.Utilities.Constants;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCore.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IRoleService _roleService;
        private readonly IViewRenderService _viewRenderService;

        public UserController(IUserService userService,
            IAuthorizationService authorizationService,
            IViewRenderService viewRenderService,
            IRoleService roleService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
            _roleService = roleService;
            _viewRenderService = viewRenderService;
        }

        #region Index

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Login/Index");
            const string keyword = "";
            const int currentPage = 1;
            const int pageSize = 10;
            var lstObj = _userService.GetAllPagingAsync(keyword, currentPage, pageSize);

            var model = new PageAppUserViewModel
            {
                ListAppUserViewModels = lstObj.Results,
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
        /// <returns></returns>
        [HttpPost]
        public IActionResult Filter(FilterDefault filter)
        {
            var data = _userService.GetAllPagingAsync(filter.Keyword, filter.CurrentPage,
                filter.PageSize);
            var listContent = _viewRenderService.RenderToStringAsync("User/_ListUser", data.Results);
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

        #region Thêm tài khoản

        /// <summary>
        /// Show popup thêm mới sản phẩm
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            var model = new AppUserViewModel
            {
                IsEdit = false,
                IsView = false
            };

            #region Danh sách quyền

            var lstRoles = _roleService.GetAllAsync();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn quyền—"
                }
            };
            lstSelect.AddRange(lstRoles.Result.Select(m => new SelectListItem
            {
                Value = m.Name,
                Text = m.Description
            }));
            model.ListAppRoleViewModels = lstSelect;

            #endregion Danh sách quyền

            var content = _viewRenderService.RenderToStringAsync("User/_AddEditModal", model);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Thêm tài khoản

        #region Sửa tài khoản

        /// <summary>
        /// Sửa thông tin sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var obj = _userService.GetById(id);
            obj.Result.IsEdit = true;

            #region Danh sách quyền

            var lstRoles = _roleService.GetAllAsync();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn quyền—"
                }
            };
            lstSelect.AddRange(lstRoles.Result.Select(m => new SelectListItem
            {
                Value = m.Name,
                Text = m.Description,
                Selected = obj.Result.ListRoles.FirstOrDefault(t=>t.Contains(m.Name)) != null
            }));
            obj.Result.ListAppRoleViewModels = lstSelect;

            #endregion Danh sách quyền

            var content = _viewRenderService.RenderToStringAsync("User/_AddEditModal", obj.Result);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Sửa tài khoản

        #region Xóa tài khoản

        /// <summary>
        /// Xóa sản phẩm theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
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
            await _userService.DeleteAsync(id);

            return Json(new JsonResponse()
            {
                Success = true,
                Message = Constants.DeleteDataSuccess
            });
        }

        #endregion Xóa tài khoản

        #region Xem thông tin

        /// <summary>
        /// Xem thông tin sản phẩm
        /// </summary>
        /// <param name="id">Mã sản phẩm</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ViewItem(string id)
        {
            var obj = _userService.GetById(id);

            obj.Result.IsEdit = false;
            obj.Result.IsView = true;

            #region Danh sách quyền

            var lstRoles = await _roleService.GetAllAsync();
            var lstSelect = new List<SelectListItem>{
                new SelectListItem{
                    Value = "",
                    Text = "—Chọn quyền—"
                }
            };
            lstSelect.AddRange(lstRoles.Select(m => new SelectListItem
            {
                Value = m.Name,
                Text = m.Description,
                //Selected = obj.Result.ListRoles.FirstOrDefault(t => t.Contains(m.Name)) != null
            }));
            obj.Result.ListAppRoleViewModels = lstSelect;

            #endregion Danh sách quyền

            var content = _viewRenderService.RenderToStringAsync("User/_AddEditModal", obj.Result);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result
            });
        }

        #endregion Xem thông tin

        #region Lưu tài khoản

        /// <summary>
        /// Save and Edit sản phẩm
        /// </summary>
        /// <param name="appUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveItem(AppUserViewModel appUserViewModel)
        {
            if (ModelState.IsValid)
            {
                var errors = new Dictionary<string, string>();
                if (appUserViewModel.IsEdit == false)
                {
                    await _userService.AddAsync(appUserViewModel);
                }
                else
                {
                    await _userService.UpdateAsync(appUserViewModel);
                }
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

        #endregion Lưu tài khoản
    }
}
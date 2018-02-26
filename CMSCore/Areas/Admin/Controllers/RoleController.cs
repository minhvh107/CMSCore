using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels.Default;
using CMSCore.Application.ViewModels.System;
using CMSCore.Authorization;
using CMSCore.Services;
using CMSCore.Utilities.Constants;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCore.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IViewRenderService _viewRenderService;

        public RoleController(IRoleService roleService,
            IAuthorizationService authorizationService,
            IViewRenderService viewRenderService)
        {
            _roleService = roleService;
            _authorizationService = authorizationService;
            _viewRenderService = viewRenderService;
        }

        #region Index

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "ROLE", Operations.Read);
            if (result.Succeeded == false)
                return new RedirectResult("/Admin/Login/Index");
            const string keyword = "";
            const int currentPage = 1;
            const int pageSize = 10;
            var lstObj = _roleService.GetAllPagingAsync(keyword, currentPage, pageSize);

            var model = new PageAppRoleViewModel
            {
                ListAppRoleVm = lstObj.Results,
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
            var data = _roleService.GetAllPagingAsync(filter.Keyword, filter.CurrentPage,
                filter.PageSize);
            var listContent = _viewRenderService.RenderToStringAsync("Role/_ListRole", data.Results);
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

        #region Thêm quyền

        /// <summary>
        /// Show popup thêm mới quyền
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Add()
        {
            var model = new AppRoleViewModel
            {
                IsEdit = false,
                IsView = false
            };

            var content = _viewRenderService.RenderToStringAsync("Role/_AddEditModal", model);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Thêm quyền

        #region Sửa quyền

        /// <summary>
        /// Sửa thông tin quyền
        /// </summary>
        /// <param name="id">Mã quyền</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Edit(Guid id)
        {
            var obj = _roleService.GetById(id);
            obj.Result.IsEdit = true;

            var content = _viewRenderService.RenderToStringAsync("Role/_AddEditModal", obj.Result);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result,
            });
        }

        #endregion Sửa quyền

        #region Xóa quyền

        /// <summary>
        /// Xóa quyền theo ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
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
            await _roleService.DeleteAsync(id);

            return Json(new JsonResponse()
            {
                Success = true,
                Message = Constants.DeleteDataSuccess
            });
        }

        #endregion Xóa quyền

        #region Xem thông tin

        /// <summary>
        /// Xem thông tin quyền
        /// </summary>
        /// <param name="id">Mã quyền</param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult ViewItem(Guid id)
        {
            var obj = _roleService.GetById(id);

            obj.Result.IsEdit = false;
            obj.Result.IsView = true;

            var content = _viewRenderService.RenderToStringAsync("Role/_AddEditModal", obj.Result);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content.Result
            });
        }

        #endregion Xem thông tin

        #region Lưu quyền

        /// <summary>
        /// Save and Edit quyền
        /// </summary>
        /// <param name="appRoleViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveItem(AppRoleViewModel appRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                var errors = new Dictionary<string, string>();
                if (appRoleViewModel.IsEdit == false)
                {
                    await _roleService.AddAsync(appRoleViewModel);
                }
                else
                {
                    await _roleService.UpdateAsync(appRoleViewModel);
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

        #endregion Lưu quyền

        [HttpPost]
        public IActionResult ListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> listPermmission, Guid roleId)
        {
            _roleService.SavePermission(listPermmission, roleId);
            return new OkResult();
        }
    }
}
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Authorization;
using CMSCore.Services;
using CMSCore.Utilities.Constants;
using CMSCore.Utilities.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCore.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        #region Default

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

        #endregion Default

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
            var lstObj = await _userService.GetAllPagingAsync(keyword, currentPage, pageSize);

            // Lấy ra user đầu tiên
            var idUserFirst = lstObj.Results.First();

            // Thông tin user đầu tiên
            var infoUserFirst = await _userService.GetById(idUserFirst.Id.ToString());

            var model = new PageAppUserViewModel
            {
                ListAppUserVm = lstObj.Results,
                ListAppRoleVm = infoUserFirst.ListViewModelRoles,
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
        public async Task<IActionResult> Filter(FilterDefault filter)
        {
            var data = await _userService.GetAllPagingAsync(filter.Keyword, filter.CurrentPage,
                filter.PageSize);

            var listContent = await _viewRenderService.RenderToStringAsync("User/_ListUser", data.Results);

            var pagination = await _viewRenderService.RenderToStringAsync("Common/_CommonPagination", data);
            return Json(new JsonResponse()
            {
                Success = true,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Message = Constants.GetDataSuccess,
                Data = listContent,
                Pagination = pagination
            });
        }

        #endregion Tìm kiếm

        #region Thêm tài khoản

        /// <summary>
        /// Show popup thêm mới sản phẩm
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            var model = new AppUserViewModel
            {
                IsEdit = false,
                IsView = false
            };

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
                Text = m.Description
            }));
            model.ListItemRoles = lstSelect;

            #endregion Danh sách quyền

            var content = await _viewRenderService.RenderToStringAsync("User/_AddEditModal", model);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content,
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
        public async Task<IActionResult> Edit(string id)
        {
            var obj = await _userService.GetById(id);
            obj.IsEdit = true;

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
                Selected = obj.ListStringRoles.FirstOrDefault(t => t.Contains(m.Name)) != null
            }));
            obj.ListItemRoles = lstSelect;

            #endregion Danh sách quyền

            var content = await _viewRenderService.RenderToStringAsync("User/_AddEditModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content,
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
            var obj = await _userService.GetById(id);

            obj.IsEdit = false;
            obj.IsView = true;

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
                Selected = obj.ListStringRoles.FirstOrDefault(t => t.Contains(m.Name)) != null
            }));
            obj.ListItemRoles = lstSelect;

            #endregion Danh sách quyền

            var content = await _viewRenderService.RenderToStringAsync("User/_AddEditModal", obj);
            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content
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
                    await _userService.CreateAsync(appUserViewModel);
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

        #region List quyền

        /// <summary>
        /// Lấy danh sách quyền theo user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetListRoles(string id)
        {
            var lstRoles = await _userService.GetById(id); // Danh sách quyền theo tên tài khoản

            var content = await _viewRenderService.RenderToStringAsync("User/_ListRoles", lstRoles.ListViewModelRoles);

            return Json(new JsonResponse
            {
                Success = true,
                Message = Constants.GetDataSuccess,
                StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                Data = content,
            });
        }

        #endregion List quyền

        #region Gán quyền

        /// <summary>
        /// Gán quyền
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> AssignRole(string userId)
        {
            try
            {
                var infoUser = await _userService.GetById(userId);
                var currentRoles = infoUser.ListViewModelRoles;

                var allRoles = await _roleService.GetAllAsync();
                var lstRoleNotAssign = allRoles.Where(m => !currentRoles.Any(p => p.Id == m.Id)).ToList();

                var content = await _viewRenderService.RenderToStringAsync("User/_AssignRoles", lstRoleNotAssign);
                return Json(new JsonResponse()
                {
                    Success = true,
                    StatusCode = Utilities.Constants.StatusCode.GetDataSuccess,
                    Message = Constants.GetDataSuccess,
                    Data = content
                });
            }
            catch (Exception e)
            {
                return Json(new JsonResponse()
                {
                    Success = false,
                    StatusCode = Utilities.Constants.StatusCode.GetDataFailed,
                    Message = e.Message
                });
            }
        }

        /// <summary>
        /// Gán quyền
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="listRoles"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AssignRole(string userId, List<string> listRoles)
        {
            try
            {
                if (listRoles.Count > 0)
                {
                    var infoUser = await _userService.GetById(userId);
                    infoUser.ListStringRoles = listRoles;
                    await _userService.UpdateAsync(infoUser);

                    var infoUserAfter = await _userService.GetById(userId);

                    var content = await _viewRenderService.RenderToStringAsync("User/_ListRoles", infoUserAfter.ListViewModelRoles);
                    return Json(new JsonResponse()
                    {
                        Success = true,
                        StatusCode = Utilities.Constants.StatusCode.SaveDataSuccess,
                        Message = Constants.SaveDataSuccess,
                        Data = content
                    });
                }
                else
                {
                    return Json(new JsonResponse()
                    {
                        Success = false,
                        StatusCode = Utilities.Constants.StatusCode.SaveDataFailed,
                        Message = "Không có dữ liệu được chọn"
                    });
                }
            }
            catch (Exception e)
            {
                return Json(new JsonResponse()
                {
                    Success = false,
                    StatusCode = Utilities.Constants.StatusCode.SaveDataFailed,
                    Message = e.Message
                });
            }
        }

        #endregion Gán quyền

        /// <summary>
        /// Xoá quyền
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> RemoveRole(string id, string userId)
        {
            try
            {
                var infoUser = await _userService.GetById(userId);
                var lstNewRoles = infoUser.ListStringRoles.Where(m => m != id).ToList();
                infoUser.ListStringRoles = lstNewRoles;

                await _userService.UpdateAsync(infoUser);

                var infoUserAfter = await _userService.GetById(userId);

                var listContent = await _viewRenderService.RenderToStringAsync("User/_ListRoles", infoUserAfter.ListViewModelRoles);
                return Json(new JsonResponse()
                {
                    Success = true,
                    StatusCode = Utilities.Constants.StatusCode.DeleteItemSuccess,
                    Message = Constants.SaveDataSuccess,
                    Data = listContent
                });
            }
            catch (Exception e)
            {
                return Json(new JsonResponse()
                {
                    Success = false,
                    StatusCode = Utilities.Constants.StatusCode.DeleteItemFailed,
                    Message = e.Message
                });
            }
        }
    }
}
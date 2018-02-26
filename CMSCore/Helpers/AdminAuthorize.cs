using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCore.Helpers
{
    public class AdminAuthorize : AuthorizeAttribute
    {
        public string FunctionId { get; set; }
        public string SubFunctionId { get; set; }
        public string ActionId { get; set; }
        public int UserTypeId { get; set; }
        public override void OnAuthorization(AuthorizationHandlerContext filterContext)
        {
            // base.OnAuthorization(filterContext);
            if (!System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                var currentUrl = filterContext.RequestContext.HttpContext.Request.Url;
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var path = currentUrl?.ToString() ?? string.Empty;
                    var _path = path.Substring(0, path.LastIndexOf('/'));
                    filterContext.Result = new JsonResult
                    {
                        Data = new JsonResponse()
                        {
                            // put whatever data you want which will be sent
                            // to the client
                            Message = "Hết thời gian làm việc. Vui lòng đăng nhập lại hệ thống.",
                            Data = "/Account/Login?redirect=" + _path,
                            StatusCode = (int)HttpStatusCode.Unauthorized,
                            Success = false
                        },
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet
                    };
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Account/Login?redirect=" + currentUrl);
                }
            }
            else if (HttpContext.Current.User.Identity.GetUserId() != null)
            {
                var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var user = userManager.FindById(HttpContext.Current.User.Identity.GetUserId());
                //var isInrole = userManager.IsInRole(user.Id, Roles);

                if (user != null)
                {
                    if (UserTypeId > 0 && user.UserTypeID != UserTypeId)
                    {
                        AccessDenied(filterContext);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(FunctionId) && !string.IsNullOrEmpty(SubFunctionId))
                        {
                            var lstUserPermission = SingletonIpl.GetInstance<AccountRepository>().GetUserPermission(user.Id, FunctionId, SubFunctionId, ActionId);
                            if (
                                lstUserPermission == null || lstUserPermission.Count == 0)
                            {
                                AccessDenied(filterContext);
                            }
                        }

                        if (!string.IsNullOrEmpty(Roles) && !userManager.IsInRole(user.Id, Roles))
                        {
                            AccessDenied(filterContext);
                        }
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult("/Account/Logout");
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/Error/NotFound");
            }
        }

        public void AccessDenied(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new JsonResult
                {
                    Data = new JsonResponse()
                    {
                        // put whatever data you want which will be sent
                        // to the client
                        Message = "Bạn không có quyền truy cập chức năng này",
                        StatusCode = (int)HttpStatusCode.NotAcceptable,
                        Success = false
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
            else
            {
                filterContext.Result = new RedirectResult("/Error/AccessDenied");
            }
        }
    }
}

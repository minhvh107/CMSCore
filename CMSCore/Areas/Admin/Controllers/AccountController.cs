using CMSCore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CMSCore.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CMSCore.Areas.Admin.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        public AccountController(SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Admin/Account/Login");
        }

        [AllowAnonymous]
        public ActionResult Login(string redirect)
        {
            if (User != null)
            {
                return new RedirectResult("/Admin/Admin/Index");
            }
            var model = new LoginViewModel
            {
                RedirecUrl = redirect,
                RememberMe = false
            };
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return new RedirectResult(model.RedirecUrl ?? "/Admin/Admin/Index");
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                // return new ObjectResult(new GenericResult(false, "Tài khoản đã bị khoá"));
                ModelState.AddModelError("UserName", "Tài khoản đã bị khoá.");
                return View();
            }
            else
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập không tồn tại trong hệ thống.");
                return View();
            }
        }
    }
}
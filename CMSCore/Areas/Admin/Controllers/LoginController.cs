using CMSCore.Data.Entities;
using CMSCore.Models.AccountViewModels;
using CMSCore.Utilities.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace CMSCore.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;

        public LoginController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
            , ILogger<LoginController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public IActionResult Index(string redirect)
        {
            if (User != null)
            {
                return new RedirectResult("/");
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
        public async Task<IActionResult> Authen(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");

                return View();
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
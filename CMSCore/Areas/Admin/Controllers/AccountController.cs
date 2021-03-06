﻿using CMSCore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CMSCore.Application.Implementation;
using CMSCore.Application.Interfaces;
using CMSCore.Extensions;
using CMSCore.Models.AccountViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace CMSCore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : BaseController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IUserService _userService;

        public AccountController(
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger,
            IUserService userService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AccountInformation()
        {
            var userId = User.GetSpecificClaim("UserId");
            var model = await _userService.GetById(userId);
            return View(model);
        }


        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Admin/Account/LoginAdmin");
        }

        [AllowAnonymous]
        public ActionResult LoginAdmin(string redirect)
        {
            if (User.Identity.IsAuthenticated)
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
        public async Task<IActionResult> LoginAdmin(LoginViewModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
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
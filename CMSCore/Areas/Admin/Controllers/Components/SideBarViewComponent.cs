﻿using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Extensions;
using CMSCore.Utilities.Constants;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CMSCore.Areas.Admin.Components
{
    [Area("Admin")]
    public class SideBarViewComponent : ViewComponent
    {
        private readonly IFunctionService _functionService;

        public SideBarViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roles = ((ClaimsPrincipal)User).GetSpecificClaim("Roles");
            List<FunctionViewModel> functions;
            if (roles.Split(";").Contains(CommonConstants.AppRole.AdminRole))
            {
                functions = await _functionService.GetAllAsync(string.Empty);
            }
            else
            {
                //TODO get function by permission
                functions = new List<FunctionViewModel>();
            }
            return View(functions);
        }
    }
}
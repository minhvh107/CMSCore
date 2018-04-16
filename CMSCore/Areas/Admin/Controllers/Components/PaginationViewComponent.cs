using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels.System;
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
    public class PaginationViewComponent : ViewComponent
    {
        private readonly IFunctionService _functionService;

        public PaginationViewComponent(IFunctionService functionService)
        {
            _functionService = functionService;
        }

        
    }
}
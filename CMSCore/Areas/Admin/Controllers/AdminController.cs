using CMSCore.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace CMSCore.Areas.Admin.Controllers
{
    public class AdminController : BaseController
    {
        public IActionResult Index()
        {
            var email = User.GetSpecificClaim("Email");
            return View();
        }
    }
}
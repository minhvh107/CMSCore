using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMSCore.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
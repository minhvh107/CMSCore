using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CMSCore.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
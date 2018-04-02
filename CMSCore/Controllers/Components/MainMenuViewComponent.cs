using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CMSCore.Application.Interfaces;

namespace CMSCore.Controllers.Components
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryService _productCategoryService;

        public MainMenuViewComponent(IProductCategoryService productCategoryService)
        {
            _productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(_productCategoryService.GetAll());
        }
    }
}
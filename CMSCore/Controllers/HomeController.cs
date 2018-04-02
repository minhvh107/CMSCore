using CMSCore.Application.Interfaces;
using CMSCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CMSCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;

        private readonly IBlogService _blogService;
        private readonly ICommonService _commonService;

        public HomeController(IProductService productService,
            IBlogService blogService, ICommonService commonService,
            IProductCategoryService productCategoryService)
        {
            _blogService = blogService;
            _commonService = commonService;
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        public IActionResult Index()
        {
            ViewData["BodyClass"] = "cms-index-index cms-home-page";
            var homeVm = new HomeViewModel
            {
                HomeCategories = _productCategoryService.GetHomeCategories(5),
                HotProducts = _productService.GetHotProduct(5),
                TopSellProducts = _productService.GetLastest(5),
                LastestBlogs = _blogService.GetLastest(5),
                HomeSlides = _commonService.GetSlides("top")
            };
            return View(homeVm);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
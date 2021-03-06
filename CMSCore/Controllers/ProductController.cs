﻿using System.Linq;
using CMSCore.Application.Interfaces;
using CMSCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace CMSCore.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IColorService _colorService;
        private readonly ISizeService _sizeService;
        private readonly IConfiguration _configuration;

        public ProductController(
            IProductService productService, 
            IConfiguration configuration,
            IProductCategoryService productCategoryService,
            IColorService colorService,
            ISizeService sizeService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
            _colorService = colorService;
            _sizeService = sizeService;
        }

        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new CatalogViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = _productService.GetAllPaging(id, string.Empty, page, pageSize.Value);
            catalog.Category = _productCategoryService.GetById(id);

            return View(catalog);
        }

        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = "product-page";
            var model = new DetailViewModel
            {
                Product = _productService.GetById(id),
                RelatedProducts = _productService.GetRelatedProducts(id, 9),
                UpsellProducts = _productService.GetUpsellProducts(6),
                ProductImages = _productService.GetImages(id),
                Tags = _productService.GetProductTags(id),
                ListColors = _colorService.GetAll().Select(m=> new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList(),
                ListSizes = _sizeService.GetAll().Select(m=>new SelectListItem()
                {
                    Value = m.Id.ToString(),
                    Text = m.Name
                }).ToList()
            };
            model.Category = _productCategoryService.GetById(model.Product.CategoryId);
            return View(model);
        }

        [Route("search.html")]
        public IActionResult Search(string keyword, int? pageSize, string sortBy, int page = 1)
        {
            var catalog = new SearchResultViewModel();
            ViewData["BodyClass"] = "shop_grid_full_width_page";
            if (pageSize == null)
                pageSize = _configuration.GetValue<int>("PageSize");

            catalog.PageSize = pageSize;
            catalog.SortType = sortBy;
            catalog.Data = _productService.GetAllPaging(null, keyword, page, pageSize.Value);
            catalog.Keyword = keyword;

            return View(catalog);
        }
    }
}
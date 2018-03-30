using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels
{
    public class PageProductQuantityViewModel
    {
        public PageProductQuantityViewModel()
        {
            ListProductQuantityVm = new List<ProductQuantityViewModel>();
        }

        public int ProductId { set; get; }

        public ProductViewModel Product { get; set; }

        public List<ProductQuantityViewModel> ListProductQuantityVm { set; get; }

        public string JsonTableMyModal { set; get; }
    }
}
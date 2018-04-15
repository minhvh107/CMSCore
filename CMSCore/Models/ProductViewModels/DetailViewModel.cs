using CMSCore.Application.ViewModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSCore.Models
{
    public class DetailViewModel
    {
        public ProductViewModel Product { get; set; }

        public List<ProductViewModel> RelatedProducts { get; set; }

        public ProductCategoryViewModel Category { get; set; }

        public List<ProductImageViewModel> ProductImages { set; get; }

        public List<ProductViewModel> UpsellProducts { get; set; }

        public List<ProductViewModel> LastestProducts { get; set; }

        public List<TagViewModel> Tags { set; get; }

        public List<SelectListItem> ListColors { set; get; }
        public List<SelectListItem> ListSizes { set; get; }

        public bool Available { set; get; }
    }
}
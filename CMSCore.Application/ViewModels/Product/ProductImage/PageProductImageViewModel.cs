using System.Collections.Generic;

namespace CMSCore.Application.ViewModels
{
    public class PageProductImageViewModel
    {
        public PageProductImageViewModel()
        {
            ListProductImageVm = new List<ProductImageViewModel>();
        }

        public int ProductId { set; get; }

        public ProductViewModel Product { get; set; }

        public List<ProductImageViewModel> ListProductImageVm { set; get; }

        public string[] JsonListImage { set; get; }
    }
}
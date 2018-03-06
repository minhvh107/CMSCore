using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels.Product
{
    public class PageProductCategoryViewModel
    {
        public PageProductCategoryViewModel()
        {
            ListProductCategoryViewModels = new List<ProductCategoryViewModel>();
        }

        public PagedResultBase PagedResult { set; get; }
        public IList<ProductCategoryViewModel> ListProductCategoryViewModels { set; get; }
    }
}
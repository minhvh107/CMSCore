using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels
{
    public class PageProductViewModel
    {
        public PageProductViewModel()
        {
            ListProductViewModels = new List<ProductViewModel>();
        }

        public PagedResultBase PagedResult { set; get; }
        public IList<ProductViewModel> ListProductViewModels { set; get; }
    }
}
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels
{
    public class PageBillViewModel
    {
        public PageBillViewModel()
        {
            ListBillViewModels = new List<BillViewModel>();
        }

        public PagedResultBase PagedResult { set; get; }
        public IList<BillViewModel> ListBillViewModels { set; get; }
    }
}
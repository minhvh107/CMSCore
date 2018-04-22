using System.Collections.Generic;

namespace CMSCore.Application.ViewModels
{
    public class PageFunctionViewModel
    {
        public PageFunctionViewModel()
        {
            ListFunctionViewModels = new List<FunctionViewModel>();
        }

        public IList<FunctionViewModel> ListFunctionViewModels { set; get; }

        public AppRoleViewModel AppRoleViewModel { set; get; }
    }
}
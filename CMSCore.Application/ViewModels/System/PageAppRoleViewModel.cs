using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels.System
{
    public class PageAppRoleViewModel
    {
        public PageAppRoleViewModel()
        {
            ListAppRoleViewModels = new List<AppRoleViewModel>();
        }

        public PagedResultBase PagedResult { set; get; }
        public IList<AppRoleViewModel> ListAppRoleViewModels { set; get; }
    }
}
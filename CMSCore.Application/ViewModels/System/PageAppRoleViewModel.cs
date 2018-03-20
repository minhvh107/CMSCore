using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels
{
    public class PageAppRoleViewModel
    {
        public PageAppRoleViewModel()
        {
            ListAppRoleVm = new List<AppRoleViewModel>();
        }

        public PagedResultBase PagedResult { set; get; }

        public IList<AppRoleViewModel> ListAppRoleVm { set; get; }
        public IList<AppUserViewModel> ListAppUserVm { set; get; }
    }
}
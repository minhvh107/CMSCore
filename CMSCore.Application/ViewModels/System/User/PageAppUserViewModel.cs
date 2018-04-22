using System;
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels
{
    public class PageAppUserViewModel
    {
        public PageAppUserViewModel()
        {
            ListAppUserVm = new List<AppUserViewModel>();
        }

        public PagedResultBase PagedResult { set; get; }
        public IList<AppUserViewModel> ListAppUserVm { set; get; }
        public IList<AppRoleViewModel> ListAppRoleVm { set; get; }
    }
}
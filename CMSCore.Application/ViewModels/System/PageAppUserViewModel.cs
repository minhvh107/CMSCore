using System;
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.ViewModels.System
{
    public class PageAppUserViewModel
    {
        public PageAppUserViewModel()
        {
            ListAppUserViewModels = new List<AppUserViewModel>();
        }

        public PagedResultBase PagedResult { set; get; }
        public IList<AppUserViewModel> ListAppUserViewModels { set; get; }
        public IList<String> ListAppRoleViewModels { set; get; }
    }
}
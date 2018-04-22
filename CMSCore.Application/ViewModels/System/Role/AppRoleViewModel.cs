using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CMSCore.Application.ViewModels
{
    public class AppRoleViewModel
    {
        public AppRoleViewModel()
        {
            ListViewModelUsers = new List<AppUserViewModel>();
        }

        public Guid? Id { set; get; }

        [DisplayName("Tên quyền")]
        public string Name { set; get; }

        [DisplayName("Mô tả")]
        public string Description { set; get; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }

        public List<AppUserViewModel> ListViewModelUsers { set; get; }
    }
}
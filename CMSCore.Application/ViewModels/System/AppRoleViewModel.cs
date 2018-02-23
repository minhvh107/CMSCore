using System;
using System.ComponentModel;

namespace CMSCore.Application.ViewModels.System
{
    public class AppRoleViewModel
    {
        public Guid? Id { set; get; }

        [DisplayName("Tên quyền")]
        public string Name { set; get; }

        [DisplayName("Mô tả")]
        public string Description { set; get; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }
    }
}
using System;

namespace CMSCore.Application.ViewModels
{
    public class PermissionViewModel
    {
        public int Id { get; set; }

        public Guid RoleId { get; set; }

        public string FunctionId { get; set; }

        public string ActionId { get; set; }

        public AppRoleViewModel AppRole { get; set; }

        public FunctionViewModel Function { get; set; }
    }
}
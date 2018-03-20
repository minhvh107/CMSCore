using CMSCore.Infrastructure.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSCore.Data.Entities
{
    [Table("SEC_Permissions")]
    public class Permission : DomainEntity<int>
    {
        public Permission() { }
        public Permission(Guid roleId, string functionId, string actionId)
        {
            RoleId = roleId;
            FunctionId = functionId;
            ActionId = actionId;
        }

        public Guid RoleId { get; set; }

        [StringLength(128)]
        [Required]
        public string FunctionId { get; set; }

        [StringLength(128)]
        [Required]
        public string ActionId { get; set; }


        [ForeignKey("RoleId")]
        public virtual AppRole AppRole { get; set; }

        [ForeignKey("FunctionId")]
        public virtual Function Function { get; set; }

        [ForeignKey("ActionId")]
        public virtual Action Action { get; set; }
    }
}
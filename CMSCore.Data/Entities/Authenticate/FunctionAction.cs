using CMSCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSCore.Data.Entities
{
    [Table("SEC_FunctionActions")]
    public class FunctionAction : DomainEntity<int>
    {
        public string FunctionId { set; get; }
        public string ActionId { set; get; }
    }
}
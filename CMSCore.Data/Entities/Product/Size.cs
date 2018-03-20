using CMSCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSCore.Data.Entities
{
    [Table("PRD_Sizes")]
    public class Size : DomainEntity<int>
    {
        [StringLength(250)]
        public string Name
        {
            get; set;
        }
    }
}
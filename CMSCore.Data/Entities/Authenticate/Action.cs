using CMSCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSCore.Data.Entities
{
    [Table("SEC_Actions")]
    public class Action : DomainEntity<string>
    {
        public Action()
        {
        }

        public Action(string name, int sortOrder)
        {
            this.Name = name;
        }

        [Required]
        [StringLength(128)]
        public string Name { set; get; }
    }
}
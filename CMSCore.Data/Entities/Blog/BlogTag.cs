using CMSCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSCore.Data.Entities
{
    [Table("BLO_BlogTags")]
    public class BlogTag : DomainEntity<int>
    {
        public int BlogId { set; get; }

        public string TagId { set; get; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { set; get; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { set; get; }
    }
}
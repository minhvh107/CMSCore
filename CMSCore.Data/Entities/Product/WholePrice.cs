using System;
using CMSCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;
using CMSCore.Data.Enums;
using CMSCore.Data.Interfaces;

namespace CMSCore.Data.Entities
{
    [Table("PRD_WholePrices")]
    public class WholePrice : DomainEntity<int>, ISwitchable, IDateTracking, IHasSeoMetaData, IHasSoftDelete, ISortable
    {
        public int ProductId { get; set; }

        public int FromQuantity { get; set; }

        public int ToQuantity { get; set; }

        public decimal Price { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string SeoPageTitle { get; set; }

        public string SeoAlias { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

        public bool IsDelete { get; set; }

        public int SortOrder { get; set; }
    }
}
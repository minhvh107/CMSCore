using CMSCore.Data.Enums;
using CMSCore.Data.Interfaces;
using CMSCore.Infrastructure.SharedKernel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSCore.Data.Entities
{
    [Table("PRD_Sizes")]
    public class Size : DomainEntity<int>, ISwitchable, IDateTracking, IHasSeoMetaData, IHasSoftDelete, ISortable
    {
        [StringLength(250)]
        public string Name { get; set; }

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
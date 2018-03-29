using CMSCore.Infrastructure.SharedKernel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CMSCore.Data.Interfaces;
using CMSCore.Data.Enums;
using System;

namespace CMSCore.Data.Entities
{
    [Table("PRD_Colors")]
    public class Color : DomainEntity<int>, ISwitchable, IDateTracking, IHasSeoMetaData, IHasSoftDelete, ISortable
    {
        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Code { get; set; }

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
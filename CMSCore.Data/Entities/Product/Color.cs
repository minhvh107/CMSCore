using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using CMSCore.Infrastructure.SharedKernel;

namespace CMSCore.Data.Entities
{
    [Table("PRD_Colors")]
    public class Color : DomainEntity<int>
    {

        [StringLength(250)]
        public string Name
        {
            get; set;
        }

        [StringLength(250)]
        public string Code { get; set; }
    }
}

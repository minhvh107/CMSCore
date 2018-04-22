using CMSCore.Data.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMSCore.Application.ViewModels
{
    public class FunctionViewModel
    {
        public FunctionViewModel()
        {
            Children = new List<FunctionViewModel>();
        }
        public string Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Name { set; get; }

        [Required]
        [StringLength(250)]
        public string URL { set; get; }

        [StringLength(128)]
        public string ParentId { set; get; }

        public string IconCss { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }

        public List<FunctionViewModel> Children { get; set; }

        public List<ActionViewModel> ListActions { set; get; }
    }
}
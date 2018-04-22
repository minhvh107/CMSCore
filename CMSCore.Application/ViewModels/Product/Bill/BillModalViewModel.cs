using CMSCore.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace CMSCore.Application.ViewModels
{
    public class BillModalViewModel : BillViewModel
    {
        [Required(ErrorMessage = Constants.FieldRequired)]
        public string JsonTableMyModal { set; get; }
    }
}
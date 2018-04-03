using System.ComponentModel.DataAnnotations;
using CMSCore.Utilities.Constants;

namespace CMSCore.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = Constants.FieldRequired)]
        [Display(Name = "Tên đăng nhập")]
        public string Email { get; set; }

        [Required(ErrorMessage = Constants.FieldRequired)]
        [Display(Name = "Mật khẩu")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        public string RedirecUrl { get; set; }
    }
}
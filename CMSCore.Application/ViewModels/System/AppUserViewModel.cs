using CMSCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using CMSCore.Utilities.Constants;

namespace CMSCore.Application.ViewModels
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
            ListStringRoles = new List<string>();
            ListItemRoles = new List<SelectListItem>();
            ListViewModelRoles = new List<AppRoleViewModel>();
        }

        public Guid? Id { set; get; }

        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = Constants.FieldRequired)]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string FullName { set; get; }

        [DisplayName("Ngày sinh")]
        public string BirthDay { set; get; }

        [DisplayName("Email")]
        [Required(ErrorMessage = Constants.FieldRequired)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$", ErrorMessage = "Email không đúng định dạng")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string Email { set; get; }

        [DisplayName("Mật khẩu")]
        //[Required(ErrorMessage = Constants.FieldRequired)]
        [DataType(DataType.Password)]
        public string Password { set; get; }

        [DisplayName("Xác nhận mật khẩu")]
        //[Required(ErrorMessage = Constants.FieldRequired)]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu nhập lại chưa chính xác")]
        public string ConfirmPassword { set; get; }

        [DisplayName("Tài khoản")]
        [Required(ErrorMessage = Constants.FieldRequired)]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên đăng nhập chỉ được phép chứa số và chữ")]
        public string UserName { set; get; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Số điện thoại")]
        [MaxLength(50, ErrorMessage = Constants.MaxLength + " 50 ký tự")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }

        [DisplayName("Ảnh đại diện")]
        public string Avatar { get; set; }

        [DisplayName("Trạng thái")]
        public Status Status { get; set; }

        [DisplayName("Giới tính")]
        public string Gender { get; set; }

        public DateTime DateCreated { get; set; }

        public List<string> ListStringRoles { get; set; }

        public List<SelectListItem> ListItemRoles { set; get; }

        public List<AppRoleViewModel> ListViewModelRoles { set; get; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }
    }
}
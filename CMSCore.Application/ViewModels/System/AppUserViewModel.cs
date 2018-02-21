using CMSCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSCore.Application.ViewModels.System
{
    public class AppUserViewModel
    {
        public AppUserViewModel()
        {
            ListRoles = new List<string>();
        }

        public Guid? Id { set; get; }

        [DisplayName("Họ và tên")]
        public string FullName { set; get; }

        [DisplayName("Ngày sinh")]
        public string BirthDay { set; get; }

        [DisplayName("Email")]
        public string Email { set; get; }

        [DisplayName("Mật khẩu")]
        public string Password { set; get; }

        [DisplayName("Xác nhận mật khẩu")]
        public string ConfirmPassword { set; get; }

        [DisplayName("Tài khoản")]
        public string UserName { set; get; }

        [DisplayName("Địa chỉ")]
        public string Address { get; set; }

        [DisplayName("Số điện thoại")]
        public string PhoneNumber { set; get; }

        [DisplayName("Ảnh đại diện")]
        public string Avatar { get; set; }

        [DisplayName("Trạng thái")]
        public Status Status { get; set; }

        [DisplayName("Giới tính")]
        public string Gender { get; set; }

        public DateTime DateCreated { get; set; }

        public List<string> ListRoles { get; set; }

        public List<SelectListItem> ListAppRoleViewModels { set; get; } 

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }
    }
}
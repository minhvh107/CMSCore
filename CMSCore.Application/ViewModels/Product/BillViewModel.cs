using CMSCore.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CMSCore.Data.Entities;
using CMSCore.Utilities.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSCore.Application.ViewModels
{
    public class BillViewModel
    {
        public BillViewModel()
        {
            BillDetails =  new List<BillDetailViewModel>();
        }

        public int Id { set; get; }

        [Required(ErrorMessage = Constants.FieldRequired)]
        [MaxLength(256, ErrorMessage = Constants.MaxLength + " 256 ký tự")]
        [DisplayName("Tên khách hàng")]
        public string CustomerName { set; get; }

        [MaxLength(256, ErrorMessage = Constants.MaxLength + " 256 ký tự")]
        [DisplayName("Địa chỉ")]
        public string CustomerAddress { set; get; }

        [MaxLength(50, ErrorMessage = Constants.MaxLength + " 50 ký tự")]
        [DisplayName("Số điện thoại")]
        public string CustomerMobile { set; get; }

        [MaxLength(256, ErrorMessage = Constants.MaxLength + " 256 ký tự")]
        [DisplayName("Tin nhắn")]
        public string CustomerMessage { set; get; }

        [DisplayName("Tin nhắn")]
        public PaymentMethod PaymentMethod { set; get; }

        [DisplayName("Trạng thái đơn hàng")]
        public BillStatus BillStatus { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { set; get; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateModified { set; get; }

        public Status Status { set; get; } = Status.Active;

        public Guid? CustomerId { set; get; }

        public AppUserViewModel User { set; get; }

        public List<BillDetailViewModel> BillDetails { set; get; }

        public List<SelectListItem> ListPaymentMethods { set; get; }
        public List<SelectListItem> ListBillStatus { set; get; }

        public bool IsView { set; get; }
        public bool IsEdit { set; get; }

        [Required(ErrorMessage = Constants.FieldRequired)]
        public string JsonListBillDetails { set; get; }
    }
}
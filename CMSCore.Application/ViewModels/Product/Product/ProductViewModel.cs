using CMSCore.Data.Enums;
using CMSCore.Utilities.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMSCore.Application.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [DisplayName("Tên sản phẩm")]
        [Required(ErrorMessage = Constants.FieldRequired)]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string Name { get; set; }

        [DisplayName("Nhóm sản phẩm")]
        [Required(ErrorMessage = Constants.FieldRequired)]
        public int CategoryId { get; set; }

        [DisplayName("Hình ảnh")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string Image { get; set; }

        [DisplayName("Giá")]
        [Required(ErrorMessage = Constants.FieldRequired)]
        [DefaultValue(0)]
        public decimal Price { get; set; }

        [DisplayName("Giá ưu đãi")]
        public decimal? PromotionPrice { get; set; }

        [DisplayName("Giá gốc")]
        [Required(ErrorMessage = Constants.FieldRequired)]
        public decimal OriginalPrice { get; set; }

        [DisplayName("Mô tả")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string Description { get; set; }

        [DisplayName("Nội dung")]
        public string Content { get; set; }

        [DisplayName("Trang chủ")]
        public bool? HomeFlag { get; set; }

        [DisplayName("Hot")]
        public bool? HotFlag { get; set; }

        [DisplayName("Số lượng xem")]
        public int? ViewCount { get; set; }

        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string Tags { get; set; }

        [DisplayName("Đơn vị")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string Unit { get; set; }

        public ProductCategoryViewModel ProductCategory { set; get; }

        [DisplayName("SEO Tiêu đề")]
        public string SeoPageTitle { set; get; }

        [DisplayName("SEO Link")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string SeoAlias { set; get; }

        [DisplayName("SEO Từ khóa")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string SeoKeywords { set; get; }

        [DisplayName("SEO Mô tả")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string SeoDescription { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }

        public Status Status { set; get; }

        public bool IsDelete { set; get; }

        [DisplayName("Thứ tự")]
        public int SortOrder { get; set; }

        [DisplayName("Nhập file Excel")]
        [MaxLength(255, ErrorMessage = Constants.MaxLength + " 255 ký tự")]
        public string FileExcel { get; set; }

        public List<SelectListItem> ListProductCate { set; get; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }
    }
}
using CMSCore.Data.Enums;
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

        [StringLength(255)]
        [DisplayName("Tên sản phẩm")]
        [Required]
        public string Name { get; set; }

        [Required]
        [DisplayName("Nhóm sản phẩm")]
        public int CategoryId { get; set; }

        [StringLength(255)]
        [DisplayName("Hình ảnh")]
        public string Image { get; set; }

        [Required]
        [DefaultValue(0)]
        [DisplayName("Giá")]
        public decimal Price { get; set; }

        [DisplayName("Giá ưu đãi")]
        public decimal? PromotionPrice { get; set; }

        [Required]
        [DisplayName("Giá gốc")]
        public decimal OriginalPrice { get; set; }

        [StringLength(255)]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        [DisplayName("Nội dung")]
        public string Content { get; set; }

        [DisplayName("Trang chủ")]
        public bool? HomeFlag { get; set; }

        [DisplayName("Hot")]
        public bool? HotFlag { get; set; }

        [DisplayName("Số lượng xem")]
        public int? ViewCount { get; set; }

        [StringLength(255)]
        public string Tags { get; set; }

        [StringLength(255)]
        [DisplayName("Đơn vị")]
        public string Unit { get; set; }

        public ProductCategoryViewModel ProductCategory { set; get; }

        [DisplayName("SEO Tiêu đề")]
        public string SeoPageTitle { set; get; }

        [StringLength(255)]
        [DisplayName("SEO Link")]
        public string SeoAlias { set; get; }

        [StringLength(255)]
        [DisplayName("SEO Từ khóa")]
        public string SeoKeywords { set; get; }

        [StringLength(255)]
        [DisplayName("SEO Mô tả")]
        public string SeoDescription { set; get; }

        public DateTime DateCreated { set; get; }
        public DateTime DateModified { set; get; }

        public Status Status { set; get; }

        public bool IsDelete { set; get; }

        [DisplayName("Thứ tự")]
        public int SortOrder { get; set; }

        [StringLength(255)]
        [DisplayName("Nhập file Excel")]
        public string FileExcel { get; set; }

        public List<SelectListItem> ListProductCate { set; get; }

        public List<ProductQuantityViewModel> ListProductQuantityVm { set; get; }

        public string JsonTableMyModal { set; get; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }
    }
}
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSCore.Application.ViewModels
{
    public class ProductQuantityViewModel
    {
        public int ProductId { get; set; }

        public int SizeId { get; set; }

        public int ColorId { get; set; }

        public int Quantity { get; set; }

        public ProductViewModel Product { get; set; }

        public SizeViewModel Size { get; set; }

        public ColorViewModel Color { get; set; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }

        public string Guid { set; get; }

        public List<SelectListItem> ListSizes { set; get; }

        public List<SelectListItem> ListColors { set; get; }
    }
}
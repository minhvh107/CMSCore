using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CMSCore.Application.ViewModels
{
    public class BillDetailViewModel
    {
        public string Guid { set; get; }

        public int Id { set; get; }

        public int BillId { set; get; }

        public int ProductId { set; get; }

        public int Quantity { set; get; }

        public decimal Price { set; get; }

        public int ColorId { get; set; }

        public int SizeId { get; set; }

        public BillViewModel Bill { set; get; }

        public ProductViewModel Product { set; get; }

        public ColorViewModel Color { set; get; }

        public SizeViewModel Size { set; get; }

        public List<SelectListItem> ListProducts { set; get; }

        public List<SelectListItem> ListSizes { set; get; }

        public List<SelectListItem> ListColors { set; get; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }

    }
}
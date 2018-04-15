using CMSCore.Application.ViewModels;

namespace CMSCore.Models
{
    public class ShoppingCartViewModel
    {
        public ProductViewModel Product { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public int ColorId { set; get; }
        public int SizeId { set; get; }
    }
}
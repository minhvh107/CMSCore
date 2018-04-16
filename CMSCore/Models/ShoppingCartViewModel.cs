using CMSCore.Application.ViewModels;

namespace CMSCore.Models
{
    public class ShoppingCartViewModel
    {
        public ProductViewModel Product { set; get; }
        public int Quantity { set; get; }
        public decimal Price { set; get; }
        public ColorViewModel Color { set; get; }
        public SizeViewModel Size { set; get; }
    }
}
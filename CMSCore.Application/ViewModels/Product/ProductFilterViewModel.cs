using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.ViewModels.Product
{
    public class ProductFilterViewModel : PagedResultBase
    {
        public int CategoryId { set; get; }
    }
}
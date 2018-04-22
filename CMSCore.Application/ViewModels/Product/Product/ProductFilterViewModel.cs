using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.ViewModels
{
    public class ProductFilterViewModel : PagedResultBase
    {
        public int CategoryId { set; get; }
    }
}
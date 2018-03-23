using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.ViewModels
{
    public class BillFillterViewModel : PagedResultBase
    {
        public string StartDate { set; get; }
        public string EndDate { set; get; }
    }
}
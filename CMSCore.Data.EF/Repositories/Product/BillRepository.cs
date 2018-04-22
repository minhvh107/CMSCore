using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class BillRepository : EFRepository<Bill, int>, IBillRepository
    {
        public BillRepository(AppDbContext context) : base(context)
        {
        }
    }
}
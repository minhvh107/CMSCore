using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class FunctionActionRepository : EFRepository<FunctionAction, int>, IFunctionActionRepository
    {
        public FunctionActionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
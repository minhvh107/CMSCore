using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class FooterRepository : EFRepository<Footer, string>, IFooterRepository
    {
        public FooterRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
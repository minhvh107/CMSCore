using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class BlogRepository : EFRepository<Blog, int>, IBlogRepository
    {
        public BlogRepository(AppDbContext context) : base(context)
        {
        }
    }
}
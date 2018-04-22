using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class BlogTagRepository : EFRepository<BlogTag, int>, IBlogTagRepository
    {
        public BlogTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
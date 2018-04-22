using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class TagRepository : EFRepository<Tag, string>, ITagRepository
    {
        public TagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
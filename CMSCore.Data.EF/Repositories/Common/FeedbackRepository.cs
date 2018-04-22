using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class FeedbackRepository : EFRepository<Feedback, int>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}
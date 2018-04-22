using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class ContactRepository : EFRepository<Contact, string>, IContactRepository
    {
        public ContactRepository(AppDbContext context) : base(context)
        {
        }
    }
}
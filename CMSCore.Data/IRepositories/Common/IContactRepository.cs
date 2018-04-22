using CMSCore.Data.Entities;
using CMSCore.Infrastructure.Interfaces;

namespace CMSCore.Data.IRepositories
{
    public interface IContactRepository : IRepository<Contact, string>
    {
    }
}
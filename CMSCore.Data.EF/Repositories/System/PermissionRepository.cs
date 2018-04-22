using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class PermissionRepository : EFRepository<Permission, int>, IPermissionRepository
    {
        public PermissionRepository(AppDbContext context) : base(context)
        {
        }
    }
}
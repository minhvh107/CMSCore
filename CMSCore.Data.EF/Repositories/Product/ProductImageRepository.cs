using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class ProductImageRepository : EFRepository<ProductImage, int>, IProductImageRepository
    {
        public ProductImageRepository(AppDbContext context) : base(context)
        {
        }
    }
}
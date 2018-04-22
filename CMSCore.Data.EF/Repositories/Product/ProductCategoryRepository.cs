using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Data.EF.Repositories
{
    public class ProductCategoryRepository : EFRepository<ProductCategory, int>, IProductCategoryRepository
    {
        private readonly AppDbContext _context;

        public ProductCategoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public List<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(m => m.SeoAlias == alias).ToList();
        }
    }
}
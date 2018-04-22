using System.Collections.Generic;
using CMSCore.Data.Entities;
using CMSCore.Infrastructure.Interfaces;

namespace CMSCore.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory, int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}
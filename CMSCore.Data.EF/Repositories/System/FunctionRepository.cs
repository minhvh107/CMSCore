using System.Collections.Generic;
using System.Linq;
using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;

namespace CMSCore.Data.EF.Repositories
{
    public class FunctionRepository : EFRepository<Function, string>, IFunctionRepository
    {
        private readonly AppDbContext _context;
        public FunctionRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
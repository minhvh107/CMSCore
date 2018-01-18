﻿using CMSCore.Data.Entities;
using CMSCore.Infrastructure.Interfaces;

namespace CMSCore.Data.IRepositories
{
    public interface IProductRepository : IRepository<Product, int>
    {
    }
}
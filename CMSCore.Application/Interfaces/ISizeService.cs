using CMSCore.Application.ViewModels;
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.Interfaces
{
    public interface ISizeService
    {
        void Create(SizeViewModel sizeVm);

        void Update(SizeViewModel sizeVm);

        void Delete(int id);

        PagedResult<SizeViewModel> GetAllPaging(string keyword, int pageIndex, int pageSize);

        List<SizeViewModel> GetAll();

        void Save();
    }
}
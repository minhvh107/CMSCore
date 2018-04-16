using CMSCore.Application.ViewModels;
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.Interfaces
{
    public interface IColorService
    {
        void Create(ColorViewModel colorVm);

        void Update(ColorViewModel colorVm);

        void Delete(int id);

        PagedResult<ColorViewModel> GetAllPaging(string keyword, int pageIndex, int pageSize);

        List<ColorViewModel> GetAll();

        ColorViewModel GetById(int id);

        void Save();
    }
}
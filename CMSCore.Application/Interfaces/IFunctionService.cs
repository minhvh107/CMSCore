using CMSCore.Application.ViewModels.System;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMSCore.Application.Interfaces
{
    public interface IFunctionService
    {
        Task<List<FunctionViewModel>> GetAll();

        List<FunctionViewModel> GetAllByPermission(Guid userId);
    }
}
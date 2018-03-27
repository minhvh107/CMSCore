using CMSCore.Application.ViewModels;
using CMSCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMSCore.Application.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CreateAsync(AppRoleViewModel userVm);

        Task UpdateAsync(AppRoleViewModel userVm);

        Task DeleteAsync(Guid id);

        Task<List<AppRoleViewModel>> GetAllAsync();

        Task<PagedResult<AppRoleViewModel>> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppRoleViewModel> GetById(string id);

        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);

        void SavePermission(List<PermissionViewModel> permissions, Guid roleId);

        Task<bool> CheckPermission(string functionId, string action, string[] roles);
    }
}
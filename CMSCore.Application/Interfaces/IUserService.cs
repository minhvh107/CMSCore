using CMSCore.Application.ViewModels;
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CMSCore.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userVm);

        Task DeleteAsync(string id);

        Task<List<AppUserViewModel>> GetAllAsync();

        Task<PagedResult<AppUserViewModel>> GetAllPagingAsync(string keyword, int page, int pageSize);

        Task<AppUserViewModel> GetById(string id);

        Task UpdateAsync(AppUserViewModel userVm);

        Task RemoveRoleInUser(string userId, string role);
    }
}
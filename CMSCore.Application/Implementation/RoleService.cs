using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;
using CMSCore.Infrastructure.Interfaces;
using CMSCore.Utilities.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCore.Application.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(
            RoleManager<AppRole> roleManager, 
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork,
            IFunctionRepository functionRepository, 
            IPermissionRepository permissionRepository)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> AddAsync(AppRoleViewModel roleVm)
        {
            var role = new AppRole()
            {
                Name = roleVm.Name,
                Description = roleVm.Description
            };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id == functionId
                        select p;
            return query.AnyAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            await _roleManager.DeleteAsync(role);
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>().ToListAsync();
        }

        public async Task<PagedResult<AppRoleViewModel>> GetAllPagingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.Name.Contains(keyword)
                || x.Description.Contains(keyword));

            int totalRow = query.Count();
            query =  query.Skip((page - 1) * pageSize)
               .Take(pageSize);

            var data = await query.ProjectTo<AppRoleViewModel>().ToListAsync();

            var paginationSet = new PagedResult<AppRoleViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };

            return paginationSet;
        }

        public async Task<AppRoleViewModel> GetById(string id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            var roleVm = Mapper.Map<AppRole, AppRoleViewModel>(role);

            // Tất cả người dùng
            //var allUsers = await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();

            // Danh sách người dùng (có quyền trùng với quyền ban đầu)
            var lstUserInRole = await _userManager.GetUsersInRoleAsync(roleVm.Name);

            roleVm.ListViewModelUsers = Mapper.Map<List<AppUser>, List<AppUserViewModel>>(lstUserInRole.ToList());

            return roleVm;
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        select new PermissionViewModel()
                        {
                            RoleId = roleId,
                            FunctionId = f.Id
                        };
            return query.ToList();
        }

        public void SavePermission(List<PermissionViewModel> permissionVms, Guid roleId)
        {
            var permissions = Mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionVms);
            var oldPermission = _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToList();
            if (oldPermission.Count > 0)
            {
                _permissionRepository.RemoveMultiple(oldPermission);
            }
            foreach (var permission in permissions)
            {
                _permissionRepository.Add(permission);
            }
            _unitOfWork.Commit();
        }

        public async Task UpdateAsync(AppRoleViewModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            await _roleManager.UpdateAsync(role);
        }
    }
}
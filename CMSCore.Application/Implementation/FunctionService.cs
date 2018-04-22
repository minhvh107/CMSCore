using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;
using CMSCore.Data.Enums;
using CMSCore.Data.IRepositories;
using CMSCore.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSCore.Application.Implementation
{
    public class FunctionService : IFunctionService
    {
        #region Default

        private readonly IFunctionRepository _functionRepository;
        private readonly IFunctionActionRepository _functionActionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FunctionService(IMapper mapper,
            IFunctionRepository functionRepository,
            IFunctionActionRepository functionActionRepository,
            IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _functionActionRepository = functionActionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion Default

        public void Create(FunctionViewModel functionVm)
        {
            var function = _mapper.Map<Function>(functionVm);
            _functionRepository.Add(function);
        }

        public void Delete(string id)
        {
            _functionRepository.Remove(id);
        }

        public void Update(FunctionViewModel functionVm)
        {
            var function = _mapper.Map<Function>(functionVm);
            _functionRepository.Update(function);
        }

        public FunctionViewModel GetById(string id)
        {
            var function = _functionRepository.FindSingle(x => x.Id == id);
            return Mapper.Map<Function, FunctionViewModel>(function);
        }

        public async Task<List<FunctionViewModel>> GetAllAsync(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
                query = query.Where(x => x.Name.Contains(filter));
            var funsVm = await query.OrderBy(x => x.ParentId).ProjectTo<FunctionViewModel>().ToListAsync();
            var listChild = funsVm;
            funsVm.ForEach(m => m.Children.AddRange(listChild.Where(x => x.ParentId == m.Id)));
            return funsVm;
        }

        public IEnumerable<FunctionViewModel> GetAllWithParentId(string parentId)
        {
            return _functionRepository.FindAll(x => x.ParentId == parentId).ProjectTo<FunctionViewModel>();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public bool CheckExistedId(string id)
        {
            return _functionRepository.FindById(id) != null;
        }

        public void ReOrder(string sourceId, string targetId)
        {
            var source = _functionRepository.FindById(sourceId);
            var target = _functionRepository.FindById(targetId);
            int tempOrder = source.SortOrder;

            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            _functionRepository.Update(source);
            _functionRepository.Update(target);
        }

        public Task<List<FunctionActionViewModel>> GetAllFunctionAction()
        {
            return _functionActionRepository.FindAll().ProjectTo<FunctionActionViewModel>().ToListAsync();
        }

        public void UpdateParentId(string sourceId, string targetId, Dictionary<string, int> items)
        {
            //Update parent id for source
            var category = _functionRepository.FindById(sourceId);
            category.ParentId = targetId;
            _functionRepository.Update(category);

            //Get all sibling
            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _functionRepository.Update(child);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
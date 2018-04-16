using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;
using CMSCore.Data.IRepositories;
using CMSCore.Infrastructure.Interfaces;
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace CMSCore.Application.Implementation
{
    public class SizeService : ISizeService
    {
        #region Default

        private readonly ISizeRepository _sizeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SizeService(
            ISizeRepository sizeRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion Default

        public void Create(SizeViewModel sizeVm)
        {
            var size = _mapper.Map<SizeViewModel, Size>(sizeVm);
            _sizeRepository.Add(size);
        }

        public void Update(SizeViewModel sizeVm)
        {
            var size = _mapper.Map<SizeViewModel, Size>(sizeVm);
            _sizeRepository.Update(size);
        }

        public void Delete(int id)
        {
            _sizeRepository.Remove(id);
        }

        public List<SizeViewModel> GetAll()
        {
            return _sizeRepository.FindAll().ProjectTo<SizeViewModel>().ToList();
        }

        public SizeViewModel GetById(int id)
        {
            return Mapper.Map<Size, SizeViewModel>(_sizeRepository.FindById(id));
        }

        public PagedResult<SizeViewModel> GetAllPaging(string keyword, int pageIndex, int pageSize)
        {
            var query = _sizeRepository.FindAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.Name.Contains(keyword));
            }
            var totalRow = query.Count();
            var data = query.OrderByDescending(m => m.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ProjectTo<SizeViewModel>().ToList();
            return new PagedResult<SizeViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
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
    public class ColorService : IColorService
    {
        #region Default

        private readonly IColorRepository _colorRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ColorService(
            IColorRepository colorRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
        )
        {
            _colorRepository = colorRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion Default

        public void Create(ColorViewModel colorVm)
        {
            var color = _mapper.Map<ColorViewModel, Color>(colorVm);
            _colorRepository.Add(color);
        }

        public void Update(ColorViewModel colorVm)
        {
            var color = _mapper.Map<ColorViewModel, Color>(colorVm);
            _colorRepository.Update(color);
        }

        public void Delete(int id)
        {
            _colorRepository.Remove(id);
        }

        public List<ColorViewModel> GetAll()
        {
            return _colorRepository.FindAll().ProjectTo<ColorViewModel>().ToList();
        }

        public PagedResult<ColorViewModel> GetAllPaging(string keyword, int pageIndex, int pageSize)
        {
            var query = _colorRepository.FindAll();

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.Name.Contains(keyword));
            }
            var totalRow = query.Count();
            var data = query.OrderByDescending(m => m.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ProjectTo<ColorViewModel>().ToList();
            return new PagedResult<ColorViewModel>()
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
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CMSCore.Application.Interfaces;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Entities;
using CMSCore.Data.Enums;
using CMSCore.Data.IRepositories;
using CMSCore.Infrastructure.Interfaces;
using CMSCore.Utilities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;

namespace CMSCore.Application.Implementation
{
    public class BillService : IBillService, IDisposable
    {
        #region Default

        private readonly IBillRepository _billRepository;
        private readonly IBillDetailRepository _billDetailRepository;
        private readonly IColorRepository _colorRepository;
        private readonly ISizeRepository _sizeRepository;
        private readonly IProductRepository _productRepository;

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BillService(IBillRepository billRepository,
            IBillDetailRepository billDetailRepository,
            IColorRepository colorRepository,
            ISizeRepository sizeRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _billRepository = billRepository;
            _billDetailRepository = billDetailRepository;
            _colorRepository = colorRepository;
            _sizeRepository = sizeRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #endregion Default

        #region Bill

        public void Create(BillViewModel billVm)
        {
            var bill = _mapper.Map<Bill>(billVm);
            var billDetails = _mapper.Map<List<BillDetailViewModel>, List<BillDetail>>(billVm.BillDetails);
            foreach (var detail in billDetails)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;

            }
            bill.BillDetails = billDetails;

            _billRepository.Add(bill);
        }

        public void Update(BillViewModel billVm)
        {
            // mapping to bill
            var bill = _mapper.Map<BillViewModel, Bill>(billVm);

            // get bill detail
            var billDetail = bill.BillDetails;

            // new detail add
            var addBillDetail = billDetail.Where(m => m.Id == 0).ToList();

            // get detail update
            var updateBillDetail = billDetail.Where(m => m.Id != 0).ToList();

            // existed detail
            var existedBillDetail = _billDetailRepository.FindAll(m => m.BillId == billVm.Id);

            // clear db
            bill.BillDetails.Clear();

            // update bill detail
            foreach (var detail in updateBillDetail)
            {
                var productUpdate = _productRepository.FindById(detail.ProductId);
                detail.Price = productUpdate.Price;
                var detailUpdate = new BillDetail(detail.Id, detail.BillId, detail.ProductId, detail.Quantity,
                    detail.Price, detail.ColorId, detail.SizeId);
                _billDetailRepository.Update(detailUpdate);
                //bill.BillDetails.Add(detailUpdate);
            }

            // add bill detail
            foreach (var detail in addBillDetail)
            {
                var product = _productRepository.FindById(detail.ProductId);
                detail.Price = product.Price;
                var detailAdd = new BillDetail(billVm.Id, detail.ProductId, detail.Quantity,
                    detail.Price, detail.ColorId, detail.SizeId);
                _billDetailRepository.Add(detailAdd);
                //bill.BillDetails.Add(detailAdd);
            }

            // xoá các dữ liệu bill detail
            var lstDelete = existedBillDetail.Where(m=> updateBillDetail.Any(n=> n.Id == m.Id)).ToList();
            _billDetailRepository.RemoveMultiple(lstDelete);

            var billUp = new Bill(bill.Id, bill.CustomerName,bill.CustomerAddress,bill.CustomerMobile,bill.CustomerMessage,bill.BillStatus,bill.PaymentMethod,bill.Status,bill.CustomerId);
            
            _billRepository.Update(billUp);
        }

        public void Delete(int billId)
        {
            var lstDetail = _billRepository.FindById(billId);
            if (lstDetail.BillDetails != null && lstDetail.BillDetails.Count > 0)
            {
                _billDetailRepository.RemoveMultiple(lstDetail.BillDetails.ToList());
            }
            _billRepository.Remove(lstDetail);
        }

        public PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword, int pageIndex, int pageSize)
        {
            var query = _billRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                var starDateTime = DateTime.ParseExact(startDate, "dd/mm/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(m => m.DateCreated >= starDateTime);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                var endDateTime = DateTime.ParseExact(endDate, "dd/mm/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(m => m.DateCreated <= endDateTime);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(m => m.CustomerName.Contains(keyword) || m.CustomerMobile.Contains(keyword));
            }
            var totalRow = query.Count();
            var data = query.OrderByDescending(m => m.DateCreated).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ProjectTo<BillViewModel>().ToList();
            return new PagedResult<BillViewModel>()
            {
                CurrentPage = pageIndex,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public BillViewModel GetById(int billId)
        {
            var bill = _billRepository.FindById(billId);
            var billVm = _mapper.Map<Bill, BillViewModel>(bill);
            var billDetailVm = _billDetailRepository.FindAll(m => m.BillId == billId).ProjectTo<BillDetailViewModel>()
                .ToList();
            foreach (var billDetail in billDetailVm)
            {
                billDetail.Guid = Guid.NewGuid().ToString();
                billVm.BillDetails.Add(billDetail);
            }
            billVm.JsonListBillDetails = billVm.BillDetails.Count > 0 ? JsonConvert.SerializeObject(billVm.BillDetails) : "";

            return billVm;
        }

        public void UpdateStatus(int billId, BillStatus status)
        {
            var bill = _billRepository.FindById(billId);
            bill.BillStatus = status;
            _billRepository.Update(bill);
        }

        #endregion Bill

        #region Bill Detail

        public BillDetailViewModel CreateBillDetail(BillDetailViewModel billDetailVm)
        {
            var billDetail = _mapper.Map<BillDetailViewModel, BillDetail>(billDetailVm);
            _billDetailRepository.Add(billDetail);
            return billDetailVm;
        }

        public void DeleteBillDetail(int productId, int billId, int colorId, int sizeId)
        {
            var billDetail = _billDetailRepository.FindSingle(x => x.ProductId == productId && x.BillId == billId && x.ColorId == colorId && x.SizeId == sizeId);
            _billDetailRepository.Remove(billDetail);
        }

        public List<BillDetailViewModel> GetBillDetails(int billId)
        {
            return _billDetailRepository
                .FindAll(m => m.BillId == billId, c => c.Bill, c => c.Color, c => c.Size, c => c.Product)
                .ProjectTo<BillDetailViewModel>().ToList();
        }

        #endregion Bill Detail

        #region General

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<ColorViewModel> GetColors()
        {
            return _colorRepository.FindAll().ProjectTo<ColorViewModel>().ToList();
        }

        public List<SizeViewModel> GetSizes()
        {
            return _sizeRepository.FindAll().ProjectTo<SizeViewModel>().ToList();
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        #endregion General
    }
}
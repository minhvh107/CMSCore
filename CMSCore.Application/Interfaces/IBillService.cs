using CMSCore.Application.ViewModels;
using CMSCore.Data.Enums;
using CMSCore.Utilities.Dtos;
using System.Collections.Generic;

namespace CMSCore.Application.Interfaces
{
    public interface IBillService
    {
        #region Bill

        void Create(BillViewModel billVm);

        void Update(BillViewModel billVm);

        void Delete(int id);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
            int pageIndex, int pageSize);

        BillViewModel GetById(int billId);

        void UpdateStatus(int billId, BillStatus status);

        #endregion Bill

        #region Bill Detail

        BillDetailViewModel CreateBillDetail(BillDetailViewModel billDetailVm);

        void DeleteBillDetail(int productId, int billId, int colorId, int sizeId);

        List<BillDetailViewModel> GetBillDetails(int billId);

        #endregion Bill Detail

        #region General

        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();

        void Save();

        #endregion General
    }
}
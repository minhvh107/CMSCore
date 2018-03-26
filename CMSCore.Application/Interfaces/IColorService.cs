using System;
using System.Collections.Generic;
using System.Text;
using CMSCore.Application.ViewModels;
using CMSCore.Data.Enums;
using CMSCore.Utilities.Dtos;

namespace CMSCore.Application.Interfaces
{
    public interface IColorService
    {
        void Create(ColorViewModel billVm);

        void Update(BillViewModel billVm);

        PagedResult<BillViewModel> GetAllPaging(string startDate, string endDate, string keyword,
            int pageIndex, int pageSize);

        BillViewModel GetDetail(int billId);

        BillDetailViewModel CreateDetail(BillDetailViewModel billDetailVm);

        void DeleteDetail(int productId, int billId, int colorId, int sizeId);

        void UpdateStatus(int billId, BillStatus status);

        List<BillDetailViewModel> GetBillDetails(int billId);

        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();

        void Save();
    }
}

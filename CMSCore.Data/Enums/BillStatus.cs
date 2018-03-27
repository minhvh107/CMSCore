using System.ComponentModel;

namespace CMSCore.Data.Enums
{
    public enum BillStatus
    {
        [Description("Mới")]
        New,

        [Description("Đang xử lý")]
        InProgress,

        [Description("Trả lại")]
        Returned,

        [Description("Huỷ bỏ")]
        Cancelled,

        [Description("Thành công")]
        Completed
    }
}
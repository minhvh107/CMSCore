namespace CMSCore.Utilities.Constants
{
    public static class Constants
    {
        public const string GetDataSuccess = "Tải dữ liệu thành công!";
        public const string GetDataFailed = "Tải dữ liệu thất bại!";

        public const string SaveDataSuccess = "Cập nhật dữ liệu thành công!";
        public const string SaveDataFailed = "Cập nhật dữ liệu thất bại!";

        public const string DeleteDataSuccess = "Xóa dữ liệu thành công!";
        public const string DeleteDataFailed = "Xóa dữ liệu thất bại!";

        public const string DuplicateKeyData = "Trùng thông tin dữ liệu!";

        #region DataAnnotations
        public const string FieldRequired = "Dữ liệu không được để trống!";
        public const string InputDataExpression = "Ký tự nhập vào không hợp lệ";
        public const string InputDataRegex = @"^([a-zA-Z0-9_ \-\.\+\,]+)$";
        public const string MaxLength = "Dữ liệu không được vượt quá";
        public const string MustBeNumber = "Dữ liệu phải là số từ 0-9";
        public const string EmptyValue = "0.0000";
        #endregion
    }
}
using System;

namespace CMSCore.Utilities.Dtos
{
    public abstract class PagedResultBase
    {
        // trang hiện tại
        public int CurrentPage { get; set; }

        // tổng số page
        public int PageCount
        {
            get
            {
                var pageCount = (double)RowCount / PageSize;
                return (int)Math.Ceiling(pageCount);
            }
            set { PageCount = value; }
        }

        // số lượng trên 1 page
        public int PageSize { get; set; }

        // tổng số
        public int RowCount { get; set; }

        public int FirstRowOnPage
        {
            get
            {
                return (CurrentPage - 1) * PageSize + 1;
            }
        }

        public int LastRowOnPage
        {
            get
            {
                return Math.Min(CurrentPage * PageSize, RowCount);
            }
        }

        public string Keyword { get; set; }
    }
}
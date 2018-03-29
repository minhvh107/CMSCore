﻿using System;
using CMSCore.Data.Enums;

namespace CMSCore.Application.ViewModels
{
    public class SizeViewModel
    {
        public int Id { set; get; }

        public string Name { get; set; }

        public Status Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public string SeoPageTitle { get; set; }

        public string SeoAlias { get; set; }

        public string SeoKeywords { get; set; }

        public string SeoDescription { get; set; }

        public bool IsDelete { get; set; }

        public int SortOrder { get; set; }

        public bool IsEdit { set; get; }

        public bool IsView { set; get; }
    }
}
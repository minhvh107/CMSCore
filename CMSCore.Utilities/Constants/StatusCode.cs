using System;
using System.Collections.Generic;
using System.Text;

namespace CMSCore.Utilities.Constants
{
    public class StatusCode
    {
        public const int GetDataFailed = 0;
        public const int GetDataSuccess = 1;
        
        public const int SaveDataFailed = 0;
        public const int SaveDataSuccess = 1;
        public const int SaveDataDuplicate = 2;

        public const int DeleteItemFailed = 0;
        public const int DeleteItemSuccess = 1;
        public const int DeleteAnActiveItem = 2;
        

        public const int SaveDataInvalidAccessary = 3;

        public const int SaveDataError = 500;
    }
}

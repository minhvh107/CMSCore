using System;
using System.Collections.Generic;
using System.Text;

namespace CMSCore.Utilities.Helpers
{
    public class JsonResponse
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public Dictionary<string, string> Errors { get; set; }
        public string Data { get; set; }
        public string Pagination { get; set; }
        public object Result { get; set; }
    }
}

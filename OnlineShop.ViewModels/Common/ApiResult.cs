using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Common
{
    public class ApiResult<T>
    {
        public bool IsSuccessed { get; set; }

        public string Message { get; set; }

        // Chứa các value/data mà tra muốn response về
        public T ResultObj { get; set; }
    }
}
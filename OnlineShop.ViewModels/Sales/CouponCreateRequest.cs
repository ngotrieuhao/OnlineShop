using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Sales
{
    public class CouponCreateRequest
    {
        public string Code { get; set; }
        public int Count { get; set; }
        public int Promotion { get; set; }
        public string Describe { get; set; }
    }
}
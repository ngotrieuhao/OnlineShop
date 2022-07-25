using OnlineShop.ViewModels.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Sales
{
    public class UpdateOrderStatusViewModel
    {
        public int OrderId { get; set; }
        public OrderStatus status { get; set; }
    }
}

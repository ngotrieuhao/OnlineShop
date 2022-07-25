using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.System.Users
{
   public class ConfirmEmailViewModel
    {
        public string token { get; set; }
        public string email { get; set; }
    }
}

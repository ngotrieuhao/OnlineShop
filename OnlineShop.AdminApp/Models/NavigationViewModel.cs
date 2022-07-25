using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.AdminApp.Models
{
    public class NavigationViewModel
    {
        public List<LanguageViewModel> Languages { get; set; }

        public string CurrentLanguageId { get; set; }
        public string ReturnUrl { get; set; }
    }
}

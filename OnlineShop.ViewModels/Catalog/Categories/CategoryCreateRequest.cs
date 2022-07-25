using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.ViewModels.Catalog.Categories
{
    public class CategoryCreateRequest
    {
        [Display(Name = "Category Name")]
        public string Name { get; set; }
    }
}

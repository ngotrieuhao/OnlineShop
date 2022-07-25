using OnlineShop.Utilities.Constants;
using OnlineShop.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.ViewModels.Catalog.Products
{
    // create thì không cần id, vì khi create sql sẽ tự động generate id tăng dần
    public class ProductCreateRequest
    {
        [Display(Name = "Product's name")]
        public string Name { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { set; get; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Quantity")]
        public int Stock { set; get; }

        [Display(Name = "Specifications")]
        public string Description { set; get; }

        [Display(Name = "Product Description")]
        public string Details { set; get; }

        [Display(Name = "Product Image")]
        public IFormFile ThumbnailImage { get; set; }

        [Display(Name = "Full Images")]
        public IFormFile ProductImage { get; set; }

        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
    }
}
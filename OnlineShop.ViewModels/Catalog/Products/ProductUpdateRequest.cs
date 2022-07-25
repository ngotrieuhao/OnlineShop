using OnlineShop.ViewModels.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineShop.ViewModels.Catalog.Products
{
    // thường ta chỉ update các property trong translation
    public class ProductUpdateRequest
    {
        public int Id { set; get; }

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

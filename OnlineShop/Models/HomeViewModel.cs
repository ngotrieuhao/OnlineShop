using OnlineShop.ViewModels.Catalog.Categories;
using OnlineShop.ViewModels.Catalog.Products;
using OnlineShop.ViewModels.Utilities.Slides;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class HomeViewModel
    {
        public List<ProductViewModel> FeaturedProducts { get; set; }

        public List<ProductViewModel> LatestProducts { get; set; }

        public List<CategoryViewModel> Categories { get; set; }
    }
}

using OnlineShop.ViewModels.Catalog.Categories;
using OnlineShop.ViewModels.Catalog.Products;
using OnlineShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Models
{
    public class ProductCategoryViewModel : PagingRequestBase
    {
        public CategoryViewModel Category { get; set; }

        public PagedResult<ProductViewModel> Products { get; set; }
    }
}

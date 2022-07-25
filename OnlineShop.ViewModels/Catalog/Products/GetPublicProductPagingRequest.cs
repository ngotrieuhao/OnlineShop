using OnlineShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineShop.ViewModels.Catalog.Products
{
    public class GetPublicProductPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        public int? CategoryId { get; set; }
    }
}
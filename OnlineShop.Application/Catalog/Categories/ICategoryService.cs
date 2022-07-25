using OnlineShop.ViewModels.Catalog.Categories;
using OnlineShop.ViewModels.Catalog.Products;
using OnlineShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Catalog.Categories
{
    public interface ICategoryService
    {
        Task<int> Create(CategoryCreateRequest request);

        Task<int> Update(CategoryUpdateRequest request);

        Task<int> Delete(int categoryId);

        Task<PagedResult<CategoryViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<CategoryViewModel> GetById(int id);

        Task<List<CategoryViewModel>> GetAll();
    }
}
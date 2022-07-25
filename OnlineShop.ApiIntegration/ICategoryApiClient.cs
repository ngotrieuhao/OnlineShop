using OnlineShop.ViewModels.Catalog.Categories;
using OnlineShop.ViewModels.Catalog.Products;
using OnlineShop.ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.ApiIntegration
{
    public interface ICategoryApiClient
    {
        Task<PagedResult<CategoryViewModel>> GetAllPaging(GetManageProductPagingRequest request);

        Task<List<CategoryViewModel>> GetAll();

        Task<CategoryViewModel> GetById(int id);

        Task<bool> CreateCategory(CategoryCreateRequest request);

        Task<bool> UpdateCategory(CategoryUpdateRequest request);

        Task<bool> DeleteCategory(int id);
    }
}
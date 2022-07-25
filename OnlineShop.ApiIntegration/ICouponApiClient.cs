using OnlineShop.ViewModels.Catalog.Products;
using OnlineShop.ViewModels.Common;
using OnlineShop.ViewModels.Sales;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.ApiIntegration
{
    public interface ICouponApiClient
    {
        Task<List<CouponViewModel>> GetAll();

        Task<CouponViewModel> GetById(int id);

        Task<bool> CreateCoupon(CouponCreateRequest request);

        Task<bool> UpdateCoupon(CouponUpdateRequest request);

        Task<bool> DeleteCoupon(int id);

        Task<PagedResult<CouponViewModel>> GetAllPaging(GetManageProductPagingRequest request);
    }
}
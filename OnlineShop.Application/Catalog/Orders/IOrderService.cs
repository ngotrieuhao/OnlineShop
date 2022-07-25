using OnlineShop.Data.Enums;
using OnlineShop.ViewModels.Common;
using OnlineShop.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Catalog.Orders
{
    public interface IOrderService
    {
        int Create(CheckoutRequest request);

        Task<PagedResult<OrderViewModel>> GetAllPaging(GetManageOrderPagingRequest request);

        Task<ApiResult<bool>> UpdateOrderStatus(int orderId);

        Task<ApiResult<bool>> CancelOrderStatus(int orderId);

        Task<OrderByUserViewModel> GetOrderByUser(string userId);

        List<OrderDetailViewModel> GetOrderDetails(int orderId);

        OrderViewModel GetOrderById(int orderId);
    }
}
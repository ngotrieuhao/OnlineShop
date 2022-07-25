using OnlineShop.Application.Catalog.Orders;
using OnlineShop.Data.Enums;
using OnlineShop.ViewModels.Common;
using OnlineShop.ViewModels.Sales;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("createOrder")]
        [Authorize]
        public IActionResult CreateOrder([FromBody] CheckoutRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = _orderService.Create(request);

            return Ok(result);
        }

        [HttpGet("userOrders/{id}")]
        public async Task<IActionResult> GetOrderByUser(string id)
        {
            var result = await _orderService.GetOrderByUser(id);

            return Ok(result);
        }

        [HttpGet("getOrderById/{orderId}")]
        public IActionResult GetOrderById(int orderId)
        {
            var result = _orderService.GetOrderById(orderId);

            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetManageOrderPagingRequest request)
        {
            var order = await _orderService.GetAllPaging(request);
            return Ok(order);
        }

        [HttpPatch("updateOrderStatus/{id}")]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] int id)
        {
            var result = await _orderService.UpdateOrderStatus(id);
            if (result.IsSuccessed)
                return Ok();
            return BadRequest("Can't Cancel Order");
        }

        [HttpPatch("cancelOrderStatus/{id}")]
        public async Task<IActionResult> CancelOrderStatus([FromBody] int id)
        {
            var result = await _orderService.CancelOrderStatus(id);
            if (result.IsSuccessed)
                return Ok();
            return BadRequest("Can't Cancel Order");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trash.InfraStructure;
using Trash.Models.ContextModels;
using Trash.Models.TransferModels;
using Trash.Services;

namespace Trash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly UserOrderService _UserOrderService;

        public OrderController(UserOrderService orderService)
        {
            _UserOrderService = orderService;
        }

        [HttpGet("userorders")]
        public async Task<IActionResult> GetUserOrders()
        {
            long userid = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            if (userid == 0)
            {
                return BadRequest(new ApiResult() { Message = "ایدی کاربر اشتباه است", StatusCode = ApiResultStatusCode.BadRequest, IsSuccess = false });
            }

            var ApiResult = new ApiResult<List<OrderReport>>()
            {
                Data = await _UserOrderService.GetUserOrders(userid),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        public async Task<IActionResult> PostOrder(OrderRequest orderRequest)
        {
            if (orderRequest == null)
                return BadRequest(new ApiResult() { Message = "سفارش نامعتبر است", IsSuccess = false ,StatusCode = ApiResultStatusCode.BadRequest});
            var ApiResult = new ApiResult<Order>()
            {
                Data = await _UserOrderService.CreateUserOrder(orderRequest),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }
    }
}

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


        /// <summary>
        /// get history of pervious user orders ... mostly used for reports
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// get a owner userid of given order .... which user own thid order ?
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public async Task<IActionResult> GetUserIdByOrderId(long orderid)
        {
            var ApiResult = new ApiResult<object>()
            {
                Data = await _UserOrderService.GetUserIdByOrderId(orderid),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        /// <summary>
        /// usable for drivers to get the WaitingOrders
        /// </summary>
        /// <returns></returns>
        [HttpGet("waitingorders")]
        public async Task<IActionResult> GetWaitingOrders()
        {
            var ApiResult = new ApiResult<List<WaitingOrder>>()
            {
                Data = await _UserOrderService.GetWaitingOrders(),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }


        /// <summary>
        /// add a new order bu user
        /// </summary>
        /// <param name="orderRequest"></param>
        /// <returns></returns>
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trash.Data.DataContext;
using Trash.InfraStructure;
using Trash.Models.TransferModels;
using Trash.Services;

namespace Trash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly UserOrderService _UserOrderService;

        public DriverController(UserOrderService userOrderService)
        {
            _UserOrderService = userOrderService;
        }

        /// <summary>
        /// set a driver for special order .... drive confirm a order to take and get the trashes
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        [HttpPost("set")]
        public async Task<IActionResult> SetDriver(long orderid)
        {
            var roles = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToList();
            if(roles.Contains("driver"))
            {
                long driverid = int.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value);
                try
                {
                    await _UserOrderService.SetDriver(driverid, orderid);
                    var ApiResult = new ApiResult()
                    {
                        IsSuccess = true,
                        StatusCode = ApiResultStatusCode.Success,
                    };
                    return Ok(ApiResult);
                }
                catch(Exception e)
                {
                    var ApiResult = new ApiResult<Exception>()
                    {
                        Data = e,
                        IsSuccess = false,
                        StatusCode = ApiResultStatusCode.LogicError,
                        Message = e.Message
                    };
                    return BadRequest(ApiResult);
                }
            }
            return BadRequest(new ApiResult() { IsSuccess = false, Message = "کاربر دسترسی لازم را ندارد", StatusCode = ApiResultStatusCode.Forbidden });
        }

        /// <summary>
        /// used by drivers to update correctness of score and weight of user trashes ...
        /// </summary>
        /// <param name="orderReport"></param>
        /// <returns></returns>
        [HttpPut("updateuserorder")]
        public async Task<IActionResult> UpdateOrder(DriverOrderReport orderReport)
        {
            if (orderReport == null)
                return BadRequest(new ApiResult() { Message = "سفارش نامعتبر است", IsSuccess = false, StatusCode = ApiResultStatusCode.ListEmpty });
            await _UserOrderService.UpdateUserOrders(orderReport);
            var ApiResult = new ApiResult() 
            {
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }
    }
}
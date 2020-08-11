using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trash.InfraStructure;
using Trash.Models.TransferModels;
using Trash.Services;

namespace Trash.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserUsageController : ControllerBase
    {
        private readonly UserUsageService _UserUsageService;

        public UserUsageController(UserUsageService userUsageService)
        {
            _UserUsageService = userUsageService;
        }


        [HttpGet("userservices")]
        public async Task<IActionResult> GetUserServices()
        {
            int userid = int.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value);
            
            var ApiResult = new ApiResult<List<ServiceUsageReport>>()
            {
                Data = await _UserUsageService.GetUserServices(userid),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        [HttpGet("usercommodities")]
        public async Task<IActionResult> GetUserCommodities()
        {
            int userid = int.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value);

            var ApiResult = new ApiResult<List<CommodityUsageReport>>()
            {
                Data = await _UserUsageService.getUserCommodities(userid),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        [HttpGet("userlocations")]
        public async Task<IActionResult> GetUserLocations()
        {
            long userid = int.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value);

            var ApiResult = new ApiResult<List<UserLocationReport>>()
            {
                Data = await _UserUsageService.GetUserLocations(userid),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        [HttpPost("userlocations")]
        public async Task<IActionResult> AddUserLocation([FromBody] List<UserLocationReport> userLocations)
        {
            if(userLocations == null)
            {
                return BadRequest(new ApiResult() { Message = "ادرس های ارسالی معتبر نمیباشد", IsSuccess = false, StatusCode = ApiResultStatusCode.ListEmpty });
            }
            long userid = int.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value);
            await _UserUsageService.AddUserLocation(userid, userLocations);
            return Ok(new ApiResult() { IsSuccess = true, StatusCode = ApiResultStatusCode.Success });
        }

        [HttpPost("userservice")]
        public async Task<IActionResult> AddUserService(long serviceid)
        {
            if (serviceid == 0)
                return BadRequest(new ApiResult() { Message = "سرویس مورد نظر یافت نشد", IsSuccess = false , StatusCode = ApiResultStatusCode.NotFound});
            int userid = int.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value);
            var result = await _UserUsageService.AddUserService(userid, serviceid);
            if (result == null)
                return BadRequest(new ApiResult() { Message = "سرویس مورد نظر فعال نشد", IsSuccess = false, StatusCode = ApiResultStatusCode.ServerError });
            var ApiResult = new ApiResult()
            {
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = "سرویس مورد نظر با موفقیت فعال شد"
            };
            return Ok(ApiResult);
        }

        public async Task<IActionResult> AddUserCommodity(long commodityid, int number)
        {
            if (commodityid == 0 || number == 0)
                return BadRequest(new ApiResult() { Message = "سرویس مورد نظر یافت نشد", IsSuccess = false, StatusCode = ApiResultStatusCode.NotFound });
            int userid = int.Parse(HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).First().Value);
            var result = await _UserUsageService.AddUserCommodity(userid, commodityid,number);
            if (result == null)
                return BadRequest(new ApiResult() { Message = "محصول موردنظر خریداری نشد", IsSuccess = false, StatusCode = ApiResultStatusCode.ServerError });
            var ApiResult = new ApiResult()
            {
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success,
                Message = "محصول مورد نظر با موفقیت خریداری شد"
            };
            return Ok(ApiResult);
        }
    }
}
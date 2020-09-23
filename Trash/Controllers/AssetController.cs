using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Trash.InfraStructure;

namespace Trash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AssetController : ControllerBase
    {
        [HttpGet("getasset/{name}")]
        public async Task<IActionResult> GetAsset(string name)
        {
            //var image = File.OpenRead("/assets/" + name);
            var apiresult = new ApiResult<PhysicalFileResult>()
            {
                Data = new PhysicalFileResult(Environment.CurrentDirectory + "/assets/" + name, "image/jpeg"),
                Message = Environment.CurrentDirectory + "/assets/" + name,
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(apiresult);
        }

        //public string GetPhysicalPathFromRelativeUrl(string url)
        //{
        //    var path = Path.Combine(IHost.Value.WebRootPath, url.TrimStart('/').Replace("/", "\\"));
        //    return path;
        //}
    }
}
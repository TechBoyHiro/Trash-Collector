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
        public async Task<FileStreamResult> GetAsset(string name)
        {
            try
            {
                //var fg = new FileStream(Environment.CurrentDirectory + "/assets/" + name, FileMode.Open);
                //MemoryStream MS = new MemoryStream();
                //await fg.CopyToAsync(MS);
                //MS.Position = 0;

                var imagefile = System.IO.File.OpenRead(Environment.CurrentDirectory + "/assets/" + name);

                //var apiresult = new ApiResult<FileStreamResult>()
                //{
                //    //Data = new PhysicalFileResult(Environment.CurrentDirectory + "/assets/" + name, "image/jpeg"),
                //    Data = File(imagefile, "image/jpeg"),
                //    Message = Environment.CurrentDirectory + "/assets/" + name,
                //    IsSuccess = true,
                //    StatusCode = ApiResultStatusCode.Success
                //};
                //Response.ContentType = new MediaTypeHeaderValue("application/octet-stream").ToString();
                return File(imagefile, "image/jpeg");
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        //public string GetPhysicalPathFromRelativeUrl(string url)
        //{
        //    var path = Path.Combine(IHost.Value.WebRootPath, url.TrimStart('/').Replace("/", "\\"));
        //    return path;
        //}
    }
}
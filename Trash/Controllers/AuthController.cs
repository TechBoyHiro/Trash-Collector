using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService; 

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            long userid = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            var ApiResult = new ApiResult<List<User>>()
            {
                Data = await _authService.GetAll(),
                StatusCode = ApiResultStatusCode.Success,
                IsSuccess = true
            };
            return Ok(ApiResult);
        }

        [HttpGet("getbyid")]
        public async Task<IActionResult> GetById()
        {
            long userid = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            if (userid == 0)
                return BadRequest(new ApiResult() { Message = "کاربر یافت نشد", IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest });
            var ApiResult = new ApiResult<User>()
            {
                Data = await _authService.GetById(userid),
                StatusCode = ApiResultStatusCode.Success,
                IsSuccess = true
            };
            return Ok(ApiResult);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Add(NewUserRequest user)
        {
            if (user == null)
            {
                return BadRequest(new ApiResult() { Message = "کاربر یافت نشد", IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest });
            }
            if(_authService.CheckUserExist(user.Phone).Result)
            {
                return BadRequest(new ApiResult() { Message = "شماره تلفن تکراری است",IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest});
            }
            await _authService.AddUser(new User()
            {
                Name = user.Name,
                UserName = user.UserName,
                Phone = user.Phone,
                Age = user.Age,
                Email = user.Email,
                Gender = user.Gender,
                IsDeleted = false,
                RegisterDate = DateTime.UtcNow
                
            },user.Password);
            return Ok();
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _authService.GetByUserAndPass(loginRequest.UserName, loginRequest.Password);
            if(user != null)
            {
                var ApiResult = new ApiResult<string>() 
                {
                    Data = _authService.GenerateToken(user),
                    StatusCode = ApiResultStatusCode.Success,
                    IsSuccess = true
                };
                return Ok(ApiResult);
            }
            return BadRequest(new ApiResult() { Message = "نام کاربری یا رمز عبور اشتباه است" ,IsSuccess = false, StatusCode = ApiResultStatusCode.NotFound});
        }
    }
}
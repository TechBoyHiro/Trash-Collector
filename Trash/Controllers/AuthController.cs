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

        /// <summary>
        /// used to get all users
        /// </summary>
        /// <returns></returns>
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

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            return Ok("The Api Is Running Successfully ...");
        }


        [HttpGet("datetime")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDate()
        {
            var ApiResult = new ApiResult<object>()
            {
                Data = PersianDateTime.Now,
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        /// <summary>
        /// get a user by userid hidden in token
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// add a regular user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("adduser")]
        [AllowAnonymous]
        public async Task<IActionResult> AddUser(NewUserRequest user)
        {
            if (user == null)
            {
                return BadRequest(new ApiResult() { Message = "کاربر یافت نشد", IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest });
            }
            if(_authService.CheckUserExist(user.Phone,user.UserName).Result)
            {
                return BadRequest(new ApiResult() { Message = "شماره تلفن یا نام کاربری تکراری است",IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest});
            }

            var ApiResult = new ApiResult<User>()
            {
                Data = await _authService.AddUser(new User()
                {
                    Name = user.Name,
                    UserName = user.UserName,
                    Phone = user.Phone,
                    Age = user.Age,
                    Email = user.Email,
                    Gender = user.Gender,
                    IsDeleted = false,
                    RegisterDate = DateTime.UtcNow,
                    IsDriver = false,
                }, user.Password),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        /// <summary>
        /// add a driver 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Adddriver")]
        [AllowAnonymous]
        public async Task<IActionResult> AddDriver(NewUserRequest user)
        {
            if (user == null)
            {
                return BadRequest(new ApiResult() { Message = "کاربر یافت نشد", IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest });
            }
            if (_authService.CheckUserExist(user.Phone,user.UserName).Result)
            {
                return BadRequest(new ApiResult() { Message = "شماره تلفن یا نام کاربری تکراری است", IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest });
            }
            var ApiResult = new ApiResult<User>()
            {
                Data = await _authService.AddUser(new User()
                {
                    Name = user.Name,
                    UserName = user.UserName,
                    Phone = user.Phone,
                    Age = user.Age,
                    Email = user.Email,
                    Gender = user.Gender,
                    IsDeleted = false,
                    RegisterDate = DateTime.UtcNow,
                    IsDriver = true,
                    IsAvailable = true
                }, user.Password),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }


        /// <summary>
        /// login for users and drivers ...
        /// </summary>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            if(loginRequest == null)
            {
                var ApiResult = new ApiResult()
                {
                    Message = "اطلاعات وارد شده صحیح نمیباشد",
                    StatusCode = ApiResultStatusCode.Success,
                    IsSuccess = true
                };
                return BadRequest(ApiResult);
            }
            try
            {
                var user = await _authService.GetByUserAndPass(loginRequest.UserName, loginRequest.Password);

                if (user != null)
                {
                    var ApiResult = new ApiResult<string>()
                    {
                        Data = _authService.GenerateToken(user),
                        StatusCode = ApiResultStatusCode.Success,
                        IsSuccess = true
                    };
                    return Ok(ApiResult);
                }
                return BadRequest(new ApiResult() { Message = "نام کاربری یا رمز عبور اشتباه است", IsSuccess = false, StatusCode = ApiResultStatusCode.NotFound });
            }
            catch(Exception e)
            {
                var ApiResult = new ApiResult()
                {
                    Message = "نام کاربری یا رمز عبور صحیح نمیباشد",
                    StatusCode = ApiResultStatusCode.BadRequest,
                    IsSuccess = false
                };
                return BadRequest(ApiResult);
            }
        }
    }
}
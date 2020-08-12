﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trash.Services;
using Trash.InfraStructure;
using Trash.Models.TransferModels;
using Trash.Models.ContextModels;
using System.Security.Claims;

namespace Trash.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrashController : ControllerBase
    {
        private readonly TrashService _TrashService;
        private readonly TrashTypeService _TrashTypeService;

        public TrashController(TrashService trashService,TrashTypeService TrashTypeService)
        {
            _TrashService = trashService;
            _TrashTypeService = TrashTypeService;
        }

        /// <summary>
        /// get history of user trashes ... how many trashes does he/she has?
        /// </summary>
        /// <returns></returns>
        [HttpGet("usertrash")]
        public async Task<IActionResult> GetUserTrashs()
        {
            long userid = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            if (userid == 0)
            {
                return BadRequest(new ApiResult() { Message = "ایدی کاربر اشتباه است", StatusCode = ApiResultStatusCode.BadRequest, IsSuccess = false });
            }
            var ApiResult = new ApiResult<List<TrashReport>>() 
            {
                Data = await _TrashService.GetUserTrashs(userid),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }


        /// <summary>
        /// get history of user trashes filtered by trashtype ... how many trashes in given trashtype does he/she has?
        /// </summary>
        /// <param name="trashtypeid"></param>
        /// <returns></returns>
        [HttpGet("usertrashtype")]
        public async Task<IActionResult> GetUserTrashByTrashType(long trashtypeid)
        {
            long userid = int.Parse(HttpContext.User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).First().Value);
            if (userid == 0)
            {
                return BadRequest(new ApiResult() { Message = "ایدی کاربر اشتباه است", StatusCode = ApiResultStatusCode.BadRequest, IsSuccess = false });
            }
            var ApiResult = new ApiResult<List<TrashReport>>()
            {
                Data = await _TrashService.GetUserTrashsByTrashTypeId(userid, trashtypeid),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        /// <summary>
        /// get all trashtype ...
        /// </summary>
        /// <returns></returns>
        [HttpGet("trashtypes")]
        public async Task<IActionResult> GetTrashTypes()
        {
            var ApiResult = new ApiResult<List<TrashType>>()
            {
                Data = await _TrashTypeService.GetAll(),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }


        /// <summary>
        /// update the price of trashtype ....
        /// </summary>
        /// <param name="trashtypeid"></param>
        /// <param name="newprice"></param>
        /// <returns></returns>
        [HttpGet("updatetrashtype")]
        public async Task<IActionResult> UpdateTrashType(long trashtypeid,double newprice)
        {
            if (trashtypeid == 0 || newprice < 0)
            {
                return BadRequest(new ApiResult() { Message = "قیمت یا نوع اشغال اشتباه وارد شده است", StatusCode = ApiResultStatusCode.BadRequest, IsSuccess = false });
            }
            var NewTrashType = await _TrashTypeService.GetById(trashtypeid);
            NewTrashType.Price = newprice;
            var ApiResult = new ApiResult<TrashType>() 
            {
                Data = await _TrashTypeService.Update(NewTrashType),
                IsSuccess = true,
                StatusCode = ApiResultStatusCode.Success
            };
            return Ok(ApiResult);
        }

        /// <summary>
        /// set the trashes of order by user .... the correctness of scores and weights will check by driver ..... 
        /// </summary>
        /// <param name="trashRequests"></param>
        /// <returns></returns>
        //public async Task<IActionResult> SetOrderTrashes([FromBody] List<TrashRequest> trashRequests)
        //{
        //    if (trashRequests == null)
        //        return BadRequest(new ApiResult()
        //        {
        //            Message = "اشغال های وارد شده معتبر نمیباشند",
        //            IsSuccess = false,
        //            StatusCode = ApiResultStatusCode.BadRequest
        //        });
        //    await _TrashService.SetOrderTrashes(trashRequests);
        //    return Ok(new ApiResult() { IsSuccess = true, StatusCode = ApiResultStatusCode.Success });
        //}
    }
}
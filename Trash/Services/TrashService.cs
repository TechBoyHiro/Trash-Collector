using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Data.DataContext;
using Trash.Data.Repositories;
using Trash.Models.TransferModels;

namespace Trash.Services
{
    public class TrashService : Repository<Trash.Models.ContextModels.Trash>
    {
        private readonly DataContext _Context;

        public TrashService(DataContext context)
            :base(context)
        {
            _Context = context;
        }

        /// <summary>
        /// used for updating user trash when driver check the correctness of user trashes
        /// </summary>
        /// <param name="orderid"></param>
        /// <returns></returns>
        public async Task<List<Models.ContextModels.Trash>> GetTrashesByOrderId(long orderid)
        {
            return (await _Table.Where(x => x.OrderId == orderid).ToListAsync());
        }

        public async Task UpdateRange(List<Models.ContextModels.Trash> trashes)
        {
            _Table.UpdateRange(trashes);
            await _Context.SaveChangesAsync();
        }

        public async Task<List<TrashReport>> GetUserTrashs(long userid)
        {
            var Trashes = await _Table.Include(k => k.User).Include(p => p.TrashType).Include(l => l.Order).Where(x => x.UserId == userid).ToListAsync();
            var TrashReport = new List<TrashReport>();
            foreach (Models.ContextModels.Trash item in Trashes)
            {
                TrashReport.Add(new TrashReport()
                {
                    OrderId = item.OrderId,
                    Score = item.Score,
                    SubmitDate = item.Order.SubmitDate,
                    TakenDate = item.Order.TakenDate,
                    TrashTypeName = item.TrashType.Name,
                    UserId = item.UserId,
                    UserName = item.User.UserName,
                    Weight = item.Weight
                });
            }
            return TrashReport;
        }

        public async Task<List<TrashReport>> GetUserTrashsByTrashTypeId(long userid,long trashtypeid)
        {
            var Trashes = await _Table.Include(k => k.User).Include(p => p.TrashType).Include(l => l.Order).Where(x => x.UserId == userid && x.TrashTypeId == trashtypeid).ToListAsync();
            var TrashReport = new List<TrashReport>();
            foreach (Models.ContextModels.Trash item in Trashes)
            {
                TrashReport.Add(new TrashReport()
                {
                    OrderId = item.OrderId,
                    Score = item.Score,
                    SubmitDate = item.Order.SubmitDate,
                    TakenDate = item.Order.TakenDate,
                    TrashTypeName = item.TrashType.Name,
                    UserId = item.UserId,
                    UserName = item.User.UserName,
                    Weight = item.Weight
                });
            }
            return TrashReport;
        }

        public async Task<List<TrashReport>> GetUserTrashsByOrderId(long userid, long orderid)
        {
            var Trashes = await _Table.Include(k => k.User).Include(p => p.TrashType).Include(l => l.Order).Where(x => x.UserId == userid && x.OrderId == orderid).ToListAsync();
            var TrashReport = new List<TrashReport>();
            foreach (Models.ContextModels.Trash item in Trashes)
            {
                TrashReport.Add(new TrashReport()
                {
                    OrderId = item.OrderId,
                    Score = item.Score,
                    SubmitDate = item.Order.SubmitDate,
                    TakenDate = item.Order.TakenDate,
                    TrashTypeName = item.TrashType.Name,
                    UserId = item.UserId,
                    UserName = item.User.UserName,
                    Weight = item.Weight
                });
            }
            return TrashReport;
        }

        public async Task SetOrderTrashes(List<TrashRequest> trashRequests)
        {
            foreach(TrashRequest item in trashRequests)
            {
                await Add(new Models.ContextModels.Trash()
                {
                    OrderId = item.OrderId,
                    Score = item.Score,
                    TrashTypeId = item.TrashTypeId,
                    UserId = item.UserId,
                    Weight = item.Weight
                });
            }
        }
    }
}

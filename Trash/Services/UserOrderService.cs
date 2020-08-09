using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Data.DataContext;
using Trash.Data.Repositories;
using Trash.Models.ContextModels;
using Trash.Models.TransferModels;

namespace Trash.Services
{
    public class UserOrderService : Repository<Order>
    {
        private readonly DataContext _Context;
        private readonly TrashService _TrashService;

        public UserOrderService(DataContext context,TrashService trashService)
            :base(context)
        {
            _Context = context;
            _TrashService = trashService;
        }

        public async Task<List<OrderReport>> GetUserOrders(long userid)
        {
            var orders = await _Table.Include(v => v.User).Include(o => o.Driver).Include(p => p.UserLocation).Where(c => c.UserId == userid).ToListAsync();
            var OrderReports = new List<OrderReport>();
            foreach(Order item in orders)
            {
                OrderReports.Add(new OrderReport() {
                    DriverId = item.DriverId,
                    DriverUserName = item.Driver.UserName,
                    OrderAddress = item.UserLocation.Address,
                    OrderLat = item.UserLocation.Latitude,
                    OrderLong = item.UserLocation.Longitude,
                    SubmitDate = item.SubmitDate,
                    TakenDate = (DateTime)item.TakenDate,
                    TotalScore = item.TotalScore,
                    Trashes = _TrashService.GetUserTrashsByOrderId(userid,item.Id).Result,
                    UserId = item.UserId,
                    UserName = item.User.UserName
                });
            }
            return OrderReports;
        }

        public async Task<List<OrderReport>> GetUserOrdersByDate(long userid,DateTime date)
        {
            var orders = await _Table.Include(v => v.User).Include(o => o.Driver).Include(p => p.UserLocation).Where(c => c.UserId == userid && c.SubmitDate >= date).ToListAsync();
            var OrderReports = new List<OrderReport>();
            foreach (Order item in orders)
            {
                OrderReports.Add(new OrderReport()
                {
                    DriverId = item.DriverId,
                    DriverUserName = item.Driver.UserName,
                    OrderAddress = item.UserLocation.Address,
                    OrderLat = item.UserLocation.Latitude,
                    OrderLong = item.UserLocation.Longitude,
                    SubmitDate = item.SubmitDate,
                    TakenDate = (DateTime)item.TakenDate,
                    TotalScore = item.TotalScore,
                    Trashes = _TrashService.GetUserTrashsByOrderId(userid, item.Id).Result,
                    UserId = item.UserId,
                    UserName = item.User.UserName
                });
            }
            return OrderReports;
        }

        public async Task<Order> CreateUserOrder(OrderRequest orderRequest)
        {
            var Order = new Order()
            {
                Description = orderRequest.Description,
                IsTaken = false,
                SubmitDate = DateTime.Now,
                TotalScore = orderRequest.TotalScore,
                UserLocationId = orderRequest.UserLocationId,
                UserId = orderRequest.UserId,
                PaymentMethod = Models.Enums.PaymentMethod.None,
                
            };
            await Add(Order);
            return Order;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel;
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
            try
            {
                var orders = await _Table.Include(v => v.User).Include(o => o.Driver).Include(p => p.UserLocation).Where(c => c.UserId == userid).ToListAsync();
                var OrderReports = new List<OrderReport>();
                foreach (Order item in orders)
                {
                    var orderreport = new OrderReport()
                    {
                        DriverId = item.DriverId,
                        OrderAddress = item.UserLocation.Address,
                        OrderLat = item.UserLocation.Latitude,
                        OrderLong = item.UserLocation.Longitude,
                        SubmitDate = item.SubmitDate,
                        TakenDate = item.TakenDate,
                        TotalScore = item.TotalScore,
                        UserId = item.UserId,
                        UserName = item.User.UserName,
                        OrderStatus = (int)item.OrderStatus
                    };
                    orderreport.Trashes = new List<TrashReport>();
                    orderreport.Trashes = _TrashService.GetUserTrashsByOrderId(userid, item.Id).Result;
                    if (item.Driver != null)
                    {
                        orderreport.DriverId = item.DriverId;
                        orderreport.DriverUserName = item.Driver.UserName;
                    }
                    OrderReports.Add(orderreport);
                }
                return OrderReports;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// mostly usable for drivers
        /// </summary>
        /// <returns></returns>
        public async Task<List<WaitingOrder>> GetWaitingOrders()
        {
            var orders = await _Table.Include(v => v.User).Include(b => b.UserLocation).Where(x => x.OrderStatus == Models.Enums.OrderStatus.Waiting).ToListAsync();
            var waitingorders = new List<WaitingOrder>();
            foreach(Order item in orders)
            {
                waitingorders.Add(new WaitingOrder()
                {
                    Address = item.UserLocation.Address,
                    Latitude = item.UserLocation.Latitude,
                    Longitude = item.UserLocation.Longitude,
                    OrderId = item.Id,
                    SubmitDate = item.SubmitDate,
                    UserName = item.User.UserName,
                    UserPhone = item.User.Phone
                });
            }
            return waitingorders;
        }

        public async Task SetDriver(long driverid, long orderid)
        {
            var order = await _Table.Where(x => x.Id == orderid).FirstAsync();
            if(order != null)
            {
                order.DriverId = driverid;
                order.OrderStatus = Models.Enums.OrderStatus.Confirmed;
                await Update(order);
                return;
            }
            throw new ArgumentNullException();
        }

        public async Task<long> GetUserIdByOrderId(long orderid)
        {
            return await _Table.Where(x => x.Id == orderid).Select(v => v.UserId).FirstAsync();
        }

        public async Task<List<OrderReport>> GetUserOrdersByDate(long userid,DateTime date)
        {
            var orders = await _Table.Include(v => v.User).Include(o => o.Driver).Include(p => p.UserLocation).Where(c => c.UserId == userid && c.SubmitDate >= date && c.OrderStatus == Models.Enums.OrderStatus.Done).ToListAsync();
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
                    UserName = item.User.UserName,
                    OrderStatus = (int)item.OrderStatus
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
                OrderStatus = Models.Enums.OrderStatus.Waiting
            };
            Order.Id = await AddOrder(Order);
            await _TrashService.SetOrderTrashes(orderRequest.Trashes, Order.Id, orderRequest.UserId);
            return Order;
        }


        public async Task<long> AddOrder(Order order)
        {
            var obj = await _Table.AddAsync(order);
            await _Context.SaveChangesAsync();
            return obj.Entity.Id;
        }

        /// <summary>
        /// when driver check the correctness of user trashs weight and reall score
        /// </summary>
        /// <param name="orderReport"></param>
        /// <returns></returns>
        public async Task UpdateUserOrders(DriverOrderReport orderReport)
        {
            var order = await GetById(orderReport.OrderId);
            var trashes = await _TrashService.GetTrashesByOrderId(orderReport.OrderId);
            foreach(DriverTrashReport item in orderReport.TrashReports)
            {
                var trash = trashes.Where(x => x.Id == item.TrashId).First();
                trash.Weight = item.Weight;
                trash.Score = item.Score;
                await _TrashService.Update(trash);
            }
        }
    }
}

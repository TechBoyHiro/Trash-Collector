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
        private readonly UserUsageService _UserService;

        public UserOrderService(DataContext context,TrashService trashService,UserUsageService userUsageService)
            :base(context)
        {
            _Context = context;
            _TrashService = trashService;
            _UserService = userUsageService;
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

     
        public async Task<List<OrderReport>> GetDriverOrders(long driverid)
        {
            try
            {
                var waitingorders = await _Table.Include(v => v.User).Include(o => o.Driver).Include(p => p.UserLocation).Where(c => c.IsTaken == true).ToListAsync();
                var orders = waitingorders.Where(x => x.DriverId == driverid).ToList();
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
                    orderreport.Trashes = _TrashService.GetUserTrashsByOrderId(item.UserId, item.Id).Result;
                    if (item.Driver != null)
                    {
                        orderreport.DriverId = item.DriverId;
                        orderreport.DriverUserName = item.Driver.UserName;
                    }
                    OrderReports.Add(orderreport);
                }
                return OrderReports;
            }
            catch (Exception e)
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
            var orders = await _Table.Include(v => v.User).Include(b => b.UserLocation).Where(x => x.OrderStatus == Models.Enums.OrderStatus.Waiting && x.TakenDate.Year == PersianDateTime.Now.Year && x.TakenDate.Month == PersianDateTime.Now.Month && x.TakenDate.Day == PersianDateTime.Now.Day && x.IsTaken == false).ToListAsync();
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
                    TakenDate = item.TakenDate,
                    UserName = item.User.UserName,
                    UserPhone = item.User.Phone
                });
            }
            waitingorders = waitingorders.OrderBy(x => x.SubmitDate).ToList();
            return waitingorders;
        }

        public async Task SetDriver(long driverid, long orderid)
        {
            var order = await _Table.Where(x => x.Id == orderid).FirstAsync();
            if(order != null)
            {
                order.DriverId = driverid;
                order.OrderStatus = Models.Enums.OrderStatus.Taking;
                await Update(order);
                return;
            }
            throw new ArgumentNullException();
        }

        public async Task<long> GetUserIdByOrderId(long orderid)
        {
            return await _Table.Where(x => x.Id == orderid).Select(v => v.UserId).FirstAsync();
        }

        public async Task<List<OrderReport>> GetUserOrdersByDate(long userid,CustomeDateTime date)
        {
            var orders = await _Table.Include(v => v.User).Include(o => o.Driver).Include(p => p.UserLocation).Where(c => c.UserId == userid && c.SubmitDate.Year == date.Year && c.SubmitDate.Month == date.Month && c.SubmitDate.Day == date.Day && c.OrderStatus == Models.Enums.OrderStatus.Done).ToListAsync();
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
                    TakenDate = item.TakenDate,
                    TotalScore = item.TotalScore,
                    Trashes = _TrashService.GetUserTrashsByOrderId(userid, item.Id).Result,
                    UserId = item.UserId,
                    UserName = item.User.UserName,
                    OrderStatus = (int)item.OrderStatus
                });
            }
            return OrderReports;
        }

        public async Task<OrderReport> CreateUserOrder(OrderRequest orderRequest)
        {
            if (orderRequest.UserLocationId <= 0 || orderRequest.UserLocationId == null)
            {
                var userLocationId = await _UserService.AddUserLocation(orderRequest.UserId, new UserLocationReport()
                {
                    Address = orderRequest.Address,
                    Latitude = (double)orderRequest.Latitude,
                    Longitude = (double)orderRequest.Longitude,
                    Name = orderRequest.UserLocationName
                });
                
                var New_Order = new Order()
                {
                    Description = orderRequest.Description,
                    IsTaken = false,
                    SubmitDate = new CustomeDateTime() { Day = PersianDateTime.Now.Day,Month = PersianDateTime.Now.Month, Year = PersianDateTime.Now.Year},
                    TakenDate = orderRequest.TakenDate,
                    TotalScore = orderRequest.TotalScore,
                    UserLocationId = userLocationId,
                    UserId = orderRequest.UserId,
                    PaymentMethod = Models.Enums.PaymentMethod.None,
                    OrderStatus = Models.Enums.OrderStatus.Waiting
                };
                try
                {
                    New_Order.Id = await AddOrder(New_Order);
                    var OrderReport2 = new OrderReport()
                    {
                        Description = New_Order.Description,
                        OrderAddress = orderRequest.Address,
                        OrderLat = (double)orderRequest.Latitude,
                        OrderLong = (double)orderRequest.Longitude,
                        OrderStatus = (int)New_Order.OrderStatus,
                        SubmitDate = New_Order.SubmitDate,
                        TakenDate = New_Order.TakenDate,
                        TotalScore = New_Order.TotalScore,
                        UserId = New_Order.UserId
                    };
                    await _TrashService.SetOrderTrashes(orderRequest.Trashes, New_Order.Id, orderRequest.UserId);
                    return OrderReport2;
                }
                catch(Exception e)
                {
                    return null;
                }
            }
            var Order = new Order()
            {
                Description = orderRequest.Description,
                IsTaken = false,
                SubmitDate = new CustomeDateTime() { Day = PersianDateTime.Now.Day, Month = PersianDateTime.Now.Month, Year = PersianDateTime.Now.Year },
                TakenDate = orderRequest.TakenDate,
                TotalScore = orderRequest.TotalScore,
                UserLocationId = (long)orderRequest.UserLocationId,
                UserId = orderRequest.UserId,
                PaymentMethod = Models.Enums.PaymentMethod.None,
                OrderStatus = Models.Enums.OrderStatus.Waiting
            };
            Order.Id = await AddOrder(Order);
            var locationDetail = await _UserService.GetLocationDetail((long)orderRequest.UserLocationId);
            var OrderReport = new OrderReport()
            {
                Description = Order.Description,
                OrderAddress = locationDetail.Address,
                OrderLat = (double)locationDetail.Latitude,
                OrderLong = (double)locationDetail.Longitude,
                OrderStatus = (int)Order.OrderStatus,
                SubmitDate = Order.SubmitDate,
                TakenDate = Order.TakenDate,
                TotalScore = Order.TotalScore,
                UserId = Order.UserId
            };
            await _TrashService.SetOrderTrashes(orderRequest.Trashes, Order.Id, orderRequest.UserId);
            return OrderReport;
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
            order.OrderStatus = Models.Enums.OrderStatus.Done;
            int new_score = 0;
            order.IsTaken = true;
            var trashes = await _TrashService.GetTrashesByOrderId(orderReport.OrderId);
            foreach(DriverTrashReport item in orderReport.TrashReports)
            {
                var trash = trashes.Where(x => x.Id == item.TrashId).First();
                trash.Weight = item.Weight;
                trash.Score = item.Score;
                new_score += item.Score;
                await _TrashService.Update(trash);
            }
            order.TotalScore = new_score;
            _Context.Set<Order>().Update(order);
            foreach(Trash.Models.ContextModels.Trash item in trashes)
            {
                _Context.Set<Trash.Models.ContextModels.Trash>().Update(item);
            }
            await _Context.SaveChangesAsync();
        }
    }
}

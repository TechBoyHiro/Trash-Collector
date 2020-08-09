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
    public class UserUsageService 
    {
        private readonly DataContext _Context;
        private readonly DbSet<UserCommodity> _Commodity;
        private readonly DbSet<UserService> _Service;
        private readonly DbSet<UserLocation> _Location;

        public UserUsageService(DataContext context)
        {
            _Context = context;
            _Commodity = context.Set<UserCommodity>();
            _Service = context.Set<UserService>();
            _Location = context.Set<UserLocation>();
        }

        public async Task<List<ServiceUsageReport>> GetUserServices(long userid)
        {
            var services = await _Service.Include(k => k.Service).Include(p => p.User).Where(h => h.UserId == userid).ToListAsync();
            var servicesusagereport = new List<ServiceUsageReport>();
            foreach(UserService item in services)
            {
                servicesusagereport.Add(new ServiceUsageReport() {
                    FinishDate = item.Finish,
                    ServiceName = item.Service.Name,
                    StartDate = item.Start,
                    UserId = item.UserId,
                    UserName = item.User.UserName,
                    ServiceScore = item.Service.Score
                });
            }
            return servicesusagereport;
        }

        public async Task<List<CommodityUsageReport>> getUserCommodities(long userid)
        {
            var services = await _Commodity.Include(k => k.Commodity).Include(p => p.User).Where(h => h.UserId == userid).ToListAsync();
            var servicesusagereport = new List<CommodityUsageReport>();
            foreach (UserCommodity item in services)
            {
                servicesusagereport.Add(new CommodityUsageReport()
                {
                    CommodityName = item.Commodity.Name,
                    CommodityScore = item.Commodity.Score,
                    Number = (int)item.Number,
                    ReceivedDate = item.DateTime,
                    UserId = item.UserId,
                    UserName = item.User.UserName
                });
            }
            return servicesusagereport;
        }

        public async Task<List<UserLocationReport>> GetUserLocations(long userid)
        {
            var locations = await _Location.Include(k => k.User).Where(x => x.UserId == userid).ToListAsync();
            var userlocations = new List<UserLocationReport>();
            foreach(UserLocation item in locations)
            {
                userlocations.Add(new UserLocationReport()
                {
                    Address = item.Address,
                    Id = item.Id,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    UserId = item.UserId,
                    UserName = item.User.UserName
                });
            }
            return userlocations;
        }

        public async Task AddUserLocation(long userid,List<UserLocationReport> userLocations)
        {
            foreach(UserLocationReport item in userLocations)
            {
                _Context.Set<UserLocation>().Add(new UserLocation()
                {
                    Address = item.Address,
                    Description = item.Description,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    UserId = userid
                });
            }
            await _Context.SaveChangesAsync();
        }

        public async Task<UserService> AddUserService(long userid ,long serviceid)
        {
            var entity = _Context.Set<UserService>().Add(new UserService()
            {
                ServiceId = serviceid,
                Start = DateTime.Now,
                UserId = userid
            });
            await _Context.SaveChangesAsync();
            return entity.Entity;
        }

        public async Task<UserCommodity> AddUserCommodity(long userid ,long commodityid ,int number)
        {
            var entity = _Context.Set<UserCommodity>().Add(new UserCommodity()
            {
                CommodityId = commodityid,
                DateTime = DateTime.Now,
                UserId = userid,
                Number = number
            });
            var commodity = await _Context.Set<Commodity>().Where(x => x.Id == commodityid).FirstAsync();
            commodity.Stock -= number;
            _Context.Set<Commodity>().Update(commodity);
            await _Context.SaveChangesAsync();
            return entity.Entity;
        }
    }
}

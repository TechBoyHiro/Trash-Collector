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
        private readonly ServiceRepo serviceRepo;
        private readonly CommodityRepo commodityRepo;
        private readonly Repository<User> userRepo;

        public UserUsageService(DataContext context,ServiceRepo serviceRepo,CommodityRepo commodityRepo,Repository<User> userrepo)
        {
            _Context = context;
            this.commodityRepo = commodityRepo;
            this.serviceRepo = serviceRepo;
            userRepo = userrepo;
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

        public async Task<List<Commodity>> GetCommodities()
        {
            return await _Context.Set<Commodity>().ToListAsync();
        }

        public async Task<List<Service>> GetServices()
        {
            return await _Context.Set<Service>().ToListAsync();
        }

        public async Task<List<UserLocationReport>> GetUserLocations(long userid)
        {
            var locations = await _Location.Include(k => k.User).Where(x => x.UserId == userid && x.IsDeleted == false).ToListAsync();
            var userlocations = new List<UserLocationReport>();
            foreach(UserLocation item in locations)
            {
                userlocations.Add(new UserLocationReport()
                {
                    Address = item.Address,
                    Id = item.Id,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    UserId = (int)item.UserId,
                    UserName = item.User.UserName,
                    Description = item.Description,
                    Name = item.Name
                });
            }
            return userlocations;
        }

        public async Task<UserLocation> GetLocationDetail(long locationId)
        {
            return await _Location.Where(x => x.Id == locationId).FirstAsync();
        }

        public async Task<long> AddUserLocation(long userid,UserLocationReport userLocation)
        {
            var entity = _Context.Set<UserLocation>().Add(new UserLocation()
            {
                Address = userLocation.Address,
                Description = userLocation.Description,
                Latitude = userLocation.Latitude,
                Longitude = userLocation.Longitude,
                UserId = userid,
                Name = userLocation.Name,
                IsDeleted = false
            });
            await _Context.SaveChangesAsync();
            return entity.Entity.Id;
        }

        public async Task<UserService> AddUserService(long userid ,long serviceid)
        {
            var service = await serviceRepo.GetById(serviceid);
            var user = await userRepo.GetById(userid);
            if (user.Score >= service.Score)
            {
                var entity = _Context.Set<UserService>().Add(new UserService()
                {
                    ServiceId = serviceid,
                    Start = DateTime.Now,
                    UserId = userid
                });
                user.Score -= service.Score;
                await userRepo.Update(user);
                await _Context.SaveChangesAsync();
                return entity.Entity;
            }
            else return null;
        }

        public async Task<UserCommodity> AddUserCommodity(long userid ,long commodityid ,int number)
        {
            var commodity = await commodityRepo.GetById(commodityid);
            var user = await userRepo.GetById(userid);
            if (user.Score >= (commodity.Score * number) && commodity.Stock >= number)
            {
                var entity = _Context.Set<UserCommodity>().Add(new UserCommodity()
                {
                    CommodityId = commodityid,
                    DateTime = DateTime.Now,
                    UserId = userid,
                    Number = number
                });
                commodity.Stock -= number;
                user.Score -= (commodity.Score * number);
                await commodityRepo.Update(commodity);
                await userRepo.Update(user);
                await _Context.SaveChangesAsync();
                return entity.Entity;
            }
            else return null;
        }
    }
}

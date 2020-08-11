using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.ContextModels;

namespace Trash.Data.DataContext
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserCommodity>().HasOne(x => x.Commodity).WithMany(z => z.UserCommodities).HasForeignKey(o => o.CommodityId);

            modelBuilder.Entity<UserCommodity>().HasOne(x => x.User).WithMany(c => c.UserCommodities).HasForeignKey(f => f.UserId);

            modelBuilder.Entity<UserLocation>().HasOne(x => x.User).WithMany(c => c.UserLocations).HasForeignKey(k => k.UserId);

            modelBuilder.Entity<UserService>().HasOne(x => x.Service).WithMany(p => p.UserServices).HasForeignKey(o => o.ServiceId);

            modelBuilder.Entity<UserService>().HasOne(x => x.User).WithMany(l => l.UserServices).HasForeignKey(n => n.UserId);

            modelBuilder.Entity<Order>().HasOne(v => v.User).WithMany(p => p.Orders).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().HasMany(O => O.Orders).WithOne(p => p.User).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Trash.Models.ContextModels.Trash>().HasOne(c => c.User).WithMany(b => b.Trashes).OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Order>().HasOne(x => x.UserLocation).WithMany(g => g.Orders).OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<UserLocation>().HasMany(x => x.Orders).WithOne(l => l.UserLocation).OnDelete(DeleteBehavior.NoAction);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<DriverUtility> DriverUtilities { get; set; }
        public DbSet<Commodity> Commodities { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Models.ContextModels.Trash> Trashes { get; set; }
        public DbSet<TrashType> TrashTypes { get; set; }
        public DbSet<UserLocation> UserLocations { get; set; }
        public DbSet<UserService> UserServices { get; set; }
        public DbSet<UserCommodity> UserCommodities { get; set; }
    }
}
 
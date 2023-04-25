using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Trash.Infra.Context.Extensions;
using Trash.Infra.Service.Models;
using Trash.Infra.User.Infra;

namespace Trash.Infra.Context.Context
{
    public class TrashContext : DbContext
    {
        public TrashContext(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            var IEntityAssembelies = typeof(IEntity).Assembly;
            modelbuilder.RegisterEntities<IEntity>(IEntityAssembelies);

        }

        public override int SaveChanges()
        {
            _cleanString();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            _cleanString();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void _cleanString()
        {
            var changedEntities = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
            foreach (var item in changedEntities)
            {
                if (item.Entity == null)
                    continue;

                var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

                foreach (var property in properties)
                {
                    var propName = property.Name;
                    var val = (string)property.GetValue(item.Entity, null);

                    if (!string.IsNullOrEmpty(val))
                    {
                        var newVal = val.FixString();
                        if (newVal == val)
                            continue;
                        property.SetValue(item.Entity, newVal, null);
                    }
                }
            }
        }
    }
}

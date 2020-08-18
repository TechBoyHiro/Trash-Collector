using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Data.Repositories
{
    public class Repository<T> where T:class
    {
        private readonly DataContext.DataContext _Context;
        protected readonly DbSet<T> _Table;
        public Repository(DataContext.DataContext dataContext)
        {
            _Context = dataContext;
            _Table = dataContext.Set<T>();
        }

        public async Task<List<T>> GetAll()
        {
            return await _Table.ToListAsync();
        }

        public async Task<T> GetById(long id)
        {
            return await _Table.FindAsync(id);
        }

        public async Task DeleteById(long id)
        {
            _Table.Remove(await GetById(id));
            await _Context.SaveChangesAsync();
        }

        public async Task<T> Add(T Object)
        {
            var Entity = await _Table.AddAsync(Object);
            await _Context.SaveChangesAsync();
            return Entity.Entity;
        }

        public async Task<T> Update(T Object)
        {
            var Entity = _Table.Update(Object);
            await _Context.SaveChangesAsync();
            return Entity.Entity;
        }
    }
}

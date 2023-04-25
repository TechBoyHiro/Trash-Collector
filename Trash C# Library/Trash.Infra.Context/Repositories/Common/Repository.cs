using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Trash.Infra.Context.Context;
using Trash.Infra.User.Infra;

namespace Trash.Infra.Context.Repositories.Common
{
    public class Repository<TEntity> where TEntity : class , IEntity
    {
        protected readonly TrashContext _AppContext;

        protected readonly DbSet<TEntity> _Context;

        public Repository(TrashContext _appcontext)
        {
            _AppContext = _appcontext;

            _Context = _AppContext.Set<TEntity>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Data.DataContext;
using Trash.Data.Repositories;
using Trash.Models.ContextModels;

namespace Trash.Services
{
    public class TrashTypeService : Repository<TrashType>
    {
        private readonly DataContext _Context;

        public TrashTypeService(DataContext context)
            :base(context)
        {
            _Context = context;
        }
    }
}

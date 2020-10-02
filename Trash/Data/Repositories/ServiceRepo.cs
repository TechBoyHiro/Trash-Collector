using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.ContextModels;

namespace Trash.Data.Repositories
{
    public class ServiceRepo : Repository<Service>
    {
        public ServiceRepo(DataContext.DataContext context)
            :base(context)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.ContextModels;
using Trash.Data.DataContext;

namespace Trash.Data.Repositories
{
    public class CommodityRepo : Repository<Commodity>
    {
        public CommodityRepo(DataContext.DataContext context)
            : base(context)
        {
        }
    }
}

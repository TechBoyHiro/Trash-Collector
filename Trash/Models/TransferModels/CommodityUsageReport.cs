using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class CommodityUsageReport
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string CommodityName { get; set; }
        public int CommodityScore { get; set; }
        public int Number { get; set; }
        public DateTime ReceivedDate { get; set; }
    }
}

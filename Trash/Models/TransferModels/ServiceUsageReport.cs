using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class ServiceUsageReport
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public string ServiceName { get; set; }
        public int ServiceScore { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}

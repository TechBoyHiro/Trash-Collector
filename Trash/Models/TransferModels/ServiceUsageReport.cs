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
        public CustomeDateTime StartDate { get; set; }
        public CustomeDateTime? FinishDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class DriverOrderReport
    {
        public long OrderId { get; set; }
        public long UserId { get; set; }
        public List<DriverTrashReport> TrashReports { get; set; }
    }
}

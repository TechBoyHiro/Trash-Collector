using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class DriverTrashReport
    {
        public long TrashId { get; set; }
        public long? TrashTypeId { get; set; }
        public int Score { get; set; }
        public long Weight { get; set; }    // In Gram
    }
}

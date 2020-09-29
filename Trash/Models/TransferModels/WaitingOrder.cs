using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class WaitingOrder
    {
        public long OrderId { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime TakenDate { get; set; }
    }
}

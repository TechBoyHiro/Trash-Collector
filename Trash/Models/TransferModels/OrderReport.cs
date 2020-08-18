using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.Enums;

namespace Trash.Models.TransferModels
{
    public class OrderReport
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        [DefaultValue(null)]
        public long? DriverId { get; set; }
        public string DriverUserName{get;set;}
        public double OrderLat { get; set; }
        public double OrderLong { get; set; }
        public string OrderAddress { get; set; }
        public List<TrashReport> Trashes { get; set; }
        public DateTime SubmitDate { get; set; }
        [DefaultValue(null)]
        public DateTime? TakenDate { get; set; }
        public long TotalScore { get; set; }     // How Many Scores Does The Order Has
        public int OrderStatus { get; set; }
        public string Description { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class OrderReport
    {
        public long UserId { get; set; }
        public string UserName { get; set; }
        public long? DriverId { get; set; }
        public string DriverUserName{get;set;}
        public double OrderLat { get; set; }
        public double OrderLong { get; set; }
        public string OrderAddress { get; set; }
        public List<TrashReport> Trashes { get; set; }
        public DateTime SubmitDate { get; set; }
        public DateTime TakenDate { get; set; }
        public long TotalScore { get; set; }     // How Many Scores Does The Order Has
        public string Description { get; set; }
    }
}

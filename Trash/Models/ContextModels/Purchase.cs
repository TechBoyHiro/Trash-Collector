using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class Purchase
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public decimal Price { get; set; }
        public DateTime DateTime { get; set; }
        public string TrackingIssue { get; set; }
        public string Portal { get; set; }
        public int Score { get; set; }

        // Navigation Properties

        public User User { get; set; }
    }
}

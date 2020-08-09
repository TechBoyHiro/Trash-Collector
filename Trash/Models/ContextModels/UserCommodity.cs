using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class UserCommodity
    {
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public long CommodityId { get; set; }
        public int? Number { get; set; }
        public DateTime DateTime { get; set; }

        // Navigation Properties

        public User User { get; set; }
        public Commodity Commodity { get; set; }
    }
}

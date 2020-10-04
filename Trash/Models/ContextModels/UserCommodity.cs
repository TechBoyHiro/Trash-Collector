using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.TransferModels;

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
        [NotMapped]
        public CustomeDateTime DateTime { get; set; }

        // Navigation Properties

        public User User { get; set; }
        public Commodity Commodity { get; set; }
    }
}

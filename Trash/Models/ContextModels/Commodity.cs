using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class Commodity
    {
        public long Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Name { get; set; }
        [Required]
        public int Score { get; set; }
        [Required]
        public int Stock { get; set; }
        public string ImageURL { get; set; }
        // Navigation Properties

        public ICollection<UserCommodity> UserCommodities { get; set; }
    }
}

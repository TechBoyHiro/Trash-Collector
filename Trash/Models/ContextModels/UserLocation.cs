using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class UserLocation
    {
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
        [StringLength(1500)]
        public string Address { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        // Navigation Properties

        public User User { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class DriverUtility
    {
        public long Id { get; set; }
        public long DriverId { get; set; }
        public string ImageUrl { get; set; }
        [Range(1, 100)]
        public string CarModel { get; set; }
        public string CarColor { get; set; }
        [Required]
        public string CarPelak { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAvailable { get; set; }

        // Navigation Properties

        public User Driver { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class Driver
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string ImageUrl { get; set; }
        [Range(1,100)]
        public int Age { get; set; }
        public string CarModel { get; set; }
        public string CarColor { get; set; }
        [Required]
        public string CarPelak { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAvailable { get; set; }

        // Navigation Properties

        public ICollection<Order> Orders { get; set; }
    }
}

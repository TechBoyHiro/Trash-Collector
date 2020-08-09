using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class Service
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        public int Score { get; set; }

        // Navigation Properties

        public ICollection<UserService> UserServices { get; set; }
    }
}

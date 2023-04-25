using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.TransferModels;

namespace Trash.Models.ContextModels
{
    public class UserService
    {
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public long ServiceId { get; set; }
        [Required]
        public CustomeDateTime Start { get; set; }
        public CustomeDateTime? Finish { get; set; }

        // Navigation Properties

        public User User { get; set; }
        public Service Service { get; set; }
    }
}

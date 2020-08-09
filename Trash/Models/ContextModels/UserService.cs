using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class UserService
    {
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public long ServiceId { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime Start { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime? Finish { get; set; }

        // Navigation Properties

        public User User { get; set; }
        public Service Service { get; set; }
    }
}

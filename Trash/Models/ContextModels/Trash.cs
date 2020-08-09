using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.Enums;

namespace Trash.Models.ContextModels
{
    public class Trash
    {
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        [Required]
        public long OrderId { get; set; }
        [Required]
        public long TrashTypeId { get; set; }
        [Required]
        public long Weight { get; set; }    // In Gram
        public int? Score { get; set; }     // Fill By Driver 

        // Navigation Properties

        public User User { get; set; }
        public Order Order { get; set; }
        public TrashType TrashType { get; set; }
    }
}

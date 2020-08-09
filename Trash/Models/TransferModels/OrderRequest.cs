using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class OrderRequest
    {
        [Required]
        public long UserId { get; set; }
        [Required]
        public long UserLocationId { get; set; }
        public long TotalScore { get; set; }     // How Many Scores Does The Order Has
        [StringLength(500)]
        public string Description { get; set; }
    }
}

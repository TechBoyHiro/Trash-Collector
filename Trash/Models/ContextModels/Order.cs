using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Trash.Models.Enums;
using Trash.Models.TransferModels;

namespace Trash.Models.ContextModels
{
    public class Order
    {
        public long Id { get; set; }
        [Required]
        public long UserId { get; set; }
        public long? DriverId { get; set; }
        [Required]
        public long UserLocationId { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public CustomeDateTime SubmitDate { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public CustomeDateTime TakenDate { get; set; }
        public bool? IsTaken { get; set; }
        public long TotalScore { get; set; }     // How Many Scores Does The Order Has
        public OrderStatus OrderStatus { get; set; }
        public PaymentMethod? PaymentMethod { get; set; }
        [StringLength(500)]
        public string Description { get; set; }


        // Navigation Properties

        public User User { get; set; }
        public UserLocation UserLocation { get; set; }
        public User Driver { get; set; }

    }
}

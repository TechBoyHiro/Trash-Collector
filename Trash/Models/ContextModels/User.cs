using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.ContextModels
{
    public class User
    {
        public long Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [Phone]
        [StringLength(12)]
        public string Phone { get; set; }
        public long Score { get; set; }
        public bool? Gender { get; set; }
        public int? Age { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt {get;set;}
        public DateTime? RegisterDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool IsDriver { get; set; }
        public bool? IsAvailable { get; set; }

        // Navigation Properties
        
        public ICollection<Trash> Trashes { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<UserService> UserServices { get; set; }
        public ICollection<UserLocation> UserLocations { get; set; }
        public ICollection<UserCommodity> UserCommodities { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Trash.Infra.User.Infra;

namespace Trash.Infra.Service.Models
{
    // Provided Services
    [Display(Name = "Provided Services For Users")]
    public class PService : IEntity
    {
        [Required]
        public Guid Service_Id { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string ServiceName { get; set; }
        [DataType(DataType.Text)]
        public string Description { get; set; }
        public object AdditionalData { get; set; } 
        public bool IsDeleted { get; set; }
    }
}

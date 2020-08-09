using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Trash.Models.TransferModels
{
    public class NewUserRequest
    {
        [Required]
        [StringLength(255, ErrorMessage = "نام باید حداکثر 255 کاراکتر باشد")]
        public string Name { get; set; }

        [Required]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "شماره همراه باید 11 رقم باشد")]
        public string Phone { get; set; }

        [Required]
        public string UserName { get; set; }

        public bool? Gender { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 4, ErrorMessage = "رمزعبور باید بین 4 تا 255 کاراکتر باشد")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string Email { get; set; }

        [Range(1,100,ErrorMessage ="سن باید بین 0 تا 100 باشد")]
        public int? Age { get; set; }
    }
}

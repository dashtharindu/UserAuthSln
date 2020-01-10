using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserAuth.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }
        
        [Required]
        [StringLength(50,MinimumLength =4,ErrorMessage ="You must specify a password between 4 and 50 characters")]
        public string Password { get; set; }
    }
}

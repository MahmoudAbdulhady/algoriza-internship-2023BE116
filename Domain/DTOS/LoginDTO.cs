using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}

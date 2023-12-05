using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class DoctorRegisterDTO
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "FirstName is Required")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is Required")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Specialization is Required")]
        [MaxLength(100)]
        public string Specialization { get; set; }

        [Phone]
        [Required(ErrorMessage = "PhoneNumber is Required")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "DateOfBirth is Required")]
        public  string DateOfBirth { get; set; }
       

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}

using Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class DoctorUpdateDTO
    {
        [Required]
        public int doctorId { get; set; }



        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "You Should not Exceed 100 Characters")]

        public string FirstName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "You Should not Exceed 100 Characters")]

        public string LastName { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "You Should not Exceed 100 Characters")]

        public string Specilization { get; set; }

        [Phone]
        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "The phone number must be exactly 10 digits.")]
        public string PhoneNumber { get; set; }


        [Required]
        public string DateOfBirth { get; set; }

        [Required]
        public IFormFile ImageUrl { get; set; }

        [Required]
        public Gender Gender { get; set; }
    }
}

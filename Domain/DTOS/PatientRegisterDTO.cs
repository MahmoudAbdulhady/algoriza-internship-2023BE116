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
    public class PatientRegisterDTO
    {

        [Required(ErrorMessage ="Email is Required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage ="FirstName Is Required")]
        [MaxLength(100)]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "LastName Is Required")]
        [MaxLength (100)]   
        public string LastName { get; set; }

        [Phone]
        [Required]
        [RegularExpression(@"^\+\d{1,3}\d{10}$", ErrorMessage = "Invalid phone number format. and max length is 11")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "DateOfBirth Is Required")]
        public string DateOfBirth { get; set; }

  
        public IFormFile? ImageUrl { get; set; }


        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }
    }
}

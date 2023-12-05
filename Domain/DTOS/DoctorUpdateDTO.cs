using Domain.Enums;
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
        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "First Name Address is required")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Specailization is required")]
        [MaxLength(100)]
        public string Specilization { get; set; }

        [Phone]
        [Required(ErrorMessage = "Phone Number is required")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "DateOfBirth is required")]
        public string DateOfBirth { get; set; }

        [Required(ErrorMessage ="Image Is required")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage ="Gender is required")]
        public Gender Gender { get; set; }
    }
}

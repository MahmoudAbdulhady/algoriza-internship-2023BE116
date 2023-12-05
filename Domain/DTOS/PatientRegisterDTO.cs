﻿using Domain.Enums;
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
        [Required(ErrorMessage = "PhoneNumber Is Required")]
        public string PhoneNumber { get; set; }


        [Required(ErrorMessage = "DateOfBirth Is Required")]
        public string DateOfBirth { get; set; }


        public string ImageUrl { get; set; }


        [Required(ErrorMessage = "Gender Is Required")]
        public Gender Gender { get; set; }
    }
}
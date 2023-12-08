using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CustomUser : IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }


        public string? ImageUrl { get; set; }

        [Required]
        public Gender Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public AccountRole AccountRole { get; set; }


        //Navigation Property for Coupons
        public ICollection<Coupon> Coupons { get;  set; }

    }
}

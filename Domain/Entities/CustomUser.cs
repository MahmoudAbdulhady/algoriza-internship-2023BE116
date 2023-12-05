using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class CustomUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string? ImageUrl { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public AccountRole AccountRole { get; set; }



         
  

    }
}

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class DoctorDTO
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Specilization { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string ImageUrl { get; set; }
        public string Gender { get; set; }
    }
}

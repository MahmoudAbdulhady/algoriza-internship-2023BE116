using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class PatientBookingDTO
    {
        public string DoctorName { get; set; }
        public string Price { get; set; }
        public string Specailization { get; set; }
        public string PhoneNumber { get; set; }
        public string Day { get; set; }
        public string FinalPrice { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; } 
        public string Image { get; set; }
        public string BookingStatus { get; set; }
    }
}

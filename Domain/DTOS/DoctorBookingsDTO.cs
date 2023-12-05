using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class DoctorBookingsDTO
    {
        public string PatientName { get; set; }
        public string Age { get; set;}
        public string PhoneNumber { get; set; }
        public string Day { get; set; }
        public string Image { get; set; }
        public string StartTime{ get; set; }
        public string EndTime { get; set; }
    }
}

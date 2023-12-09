using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class CreateBookingDTO
    {
        public int appointmentId { get; set; }
        public string PatientId { get; set; }
        public string? CouponName { get; set; }
    }
}

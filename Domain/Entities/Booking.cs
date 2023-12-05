using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }


        //PatientId & Navigation Property 
        public string PatientId { get; set; }
        public CustomUser Patient { get; set; }


        //AppointnmentId & Navigation Property 
        public int AppointmentId { get; set; }
        public Appointement Appointement { get; set; }

        //TimeId & Navigation Property 
        public int TimeId { get; set; }
        public Time Time { get; set; }

        //BookingStatus (Default value is: Pending)
        public BookingStatus Status { get; set; } = BookingStatus.Pending;

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Time
    {
        [Key]
        public int TimeId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //Foregin Key & Naviagation Property 
        public int AppointmentId { get; set; }
        public Appointement Appointements { get; set; }



        //Navigation Property (Booking Table) 
        public ICollection<Booking> Bookings { get; set; }
    }
}

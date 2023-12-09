using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointement
    {
        [Key]
        public int AppointmentId { get; set; } // Primary Key

     

        //Doctor // Foregin Key // Navigation Property
        public int DoctorId { get; set; } 
        public Doctor Doctor { get; set; }

   

        public WeekDays Days { get; set; }



        //Navigation Property 
        public ICollection<Time> Times { get; set; }
        public Booking Booking { get; set; }


    }
}

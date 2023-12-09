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
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        //Foregin Key & Naviagation Property 
        public int AppointmentId { get; set; }
        public Appointement Appointement { get; set; }



        
    }
}

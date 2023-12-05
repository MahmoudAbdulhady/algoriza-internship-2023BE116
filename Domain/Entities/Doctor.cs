using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Doctor 
    {
        [Key]
        public int DoctorId { get; set; }// Prrimary Key
        public string UserId { get; set; } // foregin Key


        // Navigation Property (Identity Table)
        public CustomUser User { get; set; } 
        public int? Price{ get; set; }


        //Navigation Property(Apponitement Table)
        public virtual ICollection<Appointement> Appointements { get; set; }

        //(Specilization Table)
        //ForeginKey  //Navigation Property
        public int SpecializationId { get; set; }
        public Specialization Specialization { get; set; }



  


    }



}

using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class AppointmentDTO
    {
        public string DoctorName { get; set; }
        public int? Price { get; set; }
        public string Specailization { get; set; }

        public List<DayScheduleDTO> AvailableDay { get; set; }
       
      
    }
}
      

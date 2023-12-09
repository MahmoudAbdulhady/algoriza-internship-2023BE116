using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class DayAppointmentDTO
    {   
         public WeekDays Day { get; set; }
         public List<TimeDTO> TimeDTOs { get; set; }
    }
}

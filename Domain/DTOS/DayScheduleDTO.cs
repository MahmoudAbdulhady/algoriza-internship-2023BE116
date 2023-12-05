using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class DayScheduleDTO
    {
        public string Day { get; set; }
        public List<string> TimeSlots { get; set; }
    }
}

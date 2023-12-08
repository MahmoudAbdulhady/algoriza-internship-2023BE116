using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class AddAppointmentDTO
    {
        [Required]
        public int DoctorId { get; set; }
        [Required]

        public int Price { get; set; }
        [Required]

        public DaysOfTheWeek DaysOfTheWeek { get; set; }
        [Required]

        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }

    }
}

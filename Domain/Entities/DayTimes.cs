using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class DayTimes
    {
        [Key]
        public int Id { get; set; }
        public string Time { get; set; }
    }
}

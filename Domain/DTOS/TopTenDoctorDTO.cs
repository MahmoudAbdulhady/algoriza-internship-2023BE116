using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class TopTenDoctorDTO
    {
        public string FullName { get; set; }
        public string Specilization { get; set; }
        public string Image { get; set; }
        public int RequestCount { get; set; }
    }
}

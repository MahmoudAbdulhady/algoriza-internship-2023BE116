using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class TopFiveSpecalizationDTO
    {
        public string SpecalizationName { get; set; }
        public int RequestCount { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Specialization
    {
        [Key]
        public int SpecializationId { get; set; }
        public string SpecializationName { get; set; }

        //Navigation Property 
        public ICollection<Doctor> Doctors { get; set; }
    }
}

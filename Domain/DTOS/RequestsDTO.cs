using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOS
{
    public class RequestsDTO
    {
        public int NumOfPendingRequest {get; set;}
        public int NumOfCompletedRequest {get; set;}
        public int NumOfCanceledRequest {get; set;}
    }
}

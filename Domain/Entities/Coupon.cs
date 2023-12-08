using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }   
        public string CouponName { get; set; }
        public DiscountCode Code { get; set; }
        public bool IsActive { get; set; }


        //Foregin Key & Navigation Property
        public string? PatientId { get; set; }
        public CustomUser Patient { get; set; }
    }
}

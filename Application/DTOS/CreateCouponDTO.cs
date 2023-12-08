using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS
{
    public class CreateCouponDTO
    {
        [Required]
        public string CouponName { get; set; }

        [Required]
        public DiscountCode CouponCode { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}

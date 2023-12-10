using Application.DTOS;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICouponService
    {
         Task<bool> CreateCouponAsync(CreateCouponDTO model);
         Task<bool>DeactivateCouponAsync(int couponId);
         Task<bool>DeleteCouponAsync(int couponId);
         Task<bool>UpdateCouponAsync(CouponUpdateDTO model);
    }
}

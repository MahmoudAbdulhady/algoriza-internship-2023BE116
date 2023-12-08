using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon> CreateCouponAsync(Coupon coupon);
        Task<Coupon> FindCouponById(int couponId);
        Task<Coupon> DeactivateCoupon(Coupon coupon);
        Task<Coupon> DeleteCoupon(Coupon coupon);
        Task<Coupon> UpdateCoupon(Coupon coupon);
        Task<Coupon> FindCouponByName (string name);
        Task<int> GetNumberOfCompletedRequestByPatientId(string patientId);
      
    }
}

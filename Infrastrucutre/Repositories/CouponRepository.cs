using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly VeeztaDbContext _veeztaDbContext;

        public CouponRepository(VeeztaDbContext veeztaDbContext)
        {
            _veeztaDbContext = veeztaDbContext;
        }

        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
          await _veeztaDbContext.Coupons.AddAsync(coupon);
           await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }

        public async Task<Coupon> DeactivateCoupon(Coupon coupon)
        {
            coupon.IsActive = false;
            await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }

        public async Task<Coupon> DeleteCoupon(Coupon coupon)
        {
            _veeztaDbContext.Coupons.Remove(coupon);
            await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }

        public async Task<Coupon> FindCouponById(int couponId)
        {
            var foundCoupon = await _veeztaDbContext.Coupons.FirstOrDefaultAsync(c => c.CouponId == couponId);
            return foundCoupon;
        }

        public async Task<Coupon> FindCouponByName(string name)
        {
            return await _veeztaDbContext.Coupons.FirstOrDefaultAsync(c => c.CouponName == name && c.IsActive);
        }

        public async Task<int> GetNumberOfCompletedRequestByPatientId(string patientId)
        {
            var completedRequest = await _veeztaDbContext.Bookings.Where(b=> b.Status == BookingStatus.Completed && b.PatientId == patientId).CountAsync();
            return completedRequest;

        }

        public async Task<Coupon> UpdateCoupon(Coupon coupon)
        {
            _veeztaDbContext.Coupons.Update(coupon);
            await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }
    }
}

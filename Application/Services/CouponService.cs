using Application.Contracts;
using Application.DTOS;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        /// <summary>
        /// Asynchronously creates a new coupon with the provided details and adds it to the system.
        /// </summary>
        /// <param name="model">The data required to create the new coupon, including its name, code, and activation status.</param>
        /// <returns>
        /// A boolean value indicating whether the creation of the coupon was successful.
        /// </returns>
        public async Task<bool> CreateCouponAsync(CreateCouponDTO model)
        {
            var coupon = new Coupon
            {
              CouponName = model.CouponName,
              Code = model.CouponCode,
              IsActive =model.IsActive,
            };
             await _couponRepository.CreateCouponAsync(coupon);
            return true;
        }

        /// <summary>
        /// Asynchronously deactivates a coupon with the specified ID if it exists and is currently active.
        /// </summary>
        /// <param name="couponId">The unique identifier of the coupon to deactivate.</param>
        /// <returns>
        /// A boolean value indicating whether the deactivation of the coupon was successful.
        /// </returns>
        public async Task<bool> DeactivateCouponAsync(int couponId)
        {
            var foundCoupon = await _couponRepository.FindCouponById(couponId);
            if(foundCoupon == null) 
            {
                throw new Exception($"The ID Coupon: {couponId} is not found");
            }

            if(!foundCoupon.IsActive)
            {
                throw new Exception($"The ID Coupon : {couponId} is already Deactivated");
            }

            await _couponRepository.DeactivateCoupon(foundCoupon);
            return true;
        }

        /// <summary>
        /// Asynchronously deletes a coupon with the specified ID if it exists.
        /// </summary>
        /// <param name="couponId">The unique identifier of the coupon to delete.</param>
        /// <returns>
        /// A boolean value indicating whether the deletion of the coupon was successful.
        /// </returns>
        public async Task<bool> DeleteCouponAsync(int couponId)
        {
            var foundCoupon = await _couponRepository.FindCouponById(couponId);
            if (foundCoupon == null)
            {
                throw new Exception($"The ID Coupon: {couponId} is not found");
            }
            await _couponRepository.DeleteCoupon(foundCoupon);
            return true;
        }

        /// <summary>
        /// Asynchronously updates a coupon using the provided data in the specified <paramref name="model"/>.
        /// </summary>
        /// <param name="model">A <see cref="CouponUpdateDTO"/> object containing the data to update the coupon.</param>
        /// <returns>
        /// <c>true</c> if the coupon update was successful; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// This method updates the coupon based on the information provided in the <paramref name="model"/>.
        /// The coupon to update is identified by the unique properties in the <paramref name="model"/>.
        /// If the coupon is successfully updated, it returns <c>true</c>; otherwise, it returns <c>false</c>.
        /// </remarks>
        /// <seealso cref="CouponUpdateDTO"/>
        /// </summary>/// <summary>
        /// Asynchronously updates an existing coupon with the specified details.
        /// </summary>
        /// <param name="model">The model containing the updated coupon information.</param>
        /// <returns>
        /// A boolean value indicating whether the update of the coupon was successful.
        /// </returns>
        public async Task<bool> UpdateCouponAsync(CouponUpdateDTO model)
        {
            var foundCoupon = await _couponRepository.FindCouponById(model.CouponId);
            if (foundCoupon == null)
            {
                throw new Exception($"The ID Coupon: {model.CouponId} is not found");
            }

            if(foundCoupon.IsActive)
            {
                throw new Exception($"You can't update this Coupon with Id: {foundCoupon.CouponId} because it's Already An Active Coupon that may be in use with another Patient");
            }
            else
            {
                foundCoupon.CouponId = model.CouponId;
                foundCoupon.CouponName = model.CouponName;
                foundCoupon.Code = model.CouponCode;
                foundCoupon.IsActive = model.IsActive;
            }
            await _couponRepository.UpdateCoupon(foundCoupon);
            return true;
        }
    }
}

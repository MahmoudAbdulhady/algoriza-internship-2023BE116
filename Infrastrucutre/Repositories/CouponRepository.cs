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

        /// <summary>
        /// Asynchronously adds a new coupon to the database.
        /// </summary>
        /// <param name="coupon">The coupon object to be added to the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the added <see cref="Coupon"/> object after it has been saved to the database.
        /// </returns>
        /// <remarks>
        /// This method adds the provided coupon object to the Veezta database context and saves the changes asynchronously. The same coupon object is returned after it's added to the database, which includes any database-generated fields like an auto-incremented ID.
        /// </remarks>
        public async Task<Coupon> CreateCouponAsync(Coupon coupon)
        {
          await _veeztaDbContext.Coupons.AddAsync(coupon);
           await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }

        /// <summary>
        /// Asynchronously deactivates a given coupon in the database.
        /// </summary>
        /// <param name="coupon">The coupon object to be deactivated.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the deactivated <see cref="Coupon"/> object after the changes have been saved to the database.
        /// </returns>
        /// <remarks>
        /// This method sets the 'IsActive' property of the provided coupon object to false, effectively deactivating the coupon. It then saves this change to the Veezta database context asynchronously. The updated coupon object is returned, reflecting its deactivated status.
        /// </remarks>
        public async Task<Coupon> DeactivateCoupon(Coupon coupon)
        {
            coupon.IsActive = false;
            await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }

        /// <summary>
        /// Asynchronously removes a coupon from the database.
        /// </summary>
        /// <param name="coupon">The coupon object to be removed from the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the removed <see cref="Coupon"/> object after the changes have been saved to the database.
        /// </returns>
        /// <remarks>
        /// This method removes the specified coupon object from the Veezta database context and saves the changes asynchronously. The method returns the same coupon object after it is removed from the database, but this object will no longer be linked to the database context.
        /// </remarks>
        public async Task<Coupon> DeleteCoupon(Coupon coupon)
        {
            _veeztaDbContext.Coupons.Remove(coupon);
            await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }

        /// <summary>
        /// Asynchronously searches for and retrieves a coupon from the database by its unique identifier.
        /// </summary>
        /// <param name="couponId">The unique identifier of the coupon to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="Coupon"/> object representing the coupon with the specified ID, or null if no matching coupon is found.
        /// </returns>
        /// <remarks>
        /// This method utilizes the Veezta database context to search for a coupon using the provided coupon ID. It returns the first coupon that matches the given ID, or null if no such coupon is found in the database. This is useful for retrieving specific coupon details based on its ID.
        /// </remarks>
        public async Task<Coupon> FindCouponById(int couponId)
        {
            var foundCoupon = await _veeztaDbContext.Coupons.FirstOrDefaultAsync(c => c.CouponId == couponId);
            return foundCoupon;
        }

        /// <summary>
        /// Asynchronously searches for and retrieves an active coupon from the database by its name.
        /// </summary>
        /// <param name="name">The name of the coupon to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="Coupon"/> object representing the active coupon with the specified name, or null if no matching active coupon is found.
        /// </returns>
        /// <remarks>
        /// This method searches the Veezta database context for an active coupon that matches the provided name. It returns the first active coupon that matches the name, or null if no such active coupon is found. This is useful for retrieving specific coupon details based on its name, particularly when only considering active coupons.
        /// </remarks>
        public async Task<Coupon> FindCouponByName(string name)
        {
            return await _veeztaDbContext.Coupons.FirstOrDefaultAsync(c => c.CouponName == name && c.IsActive);
        }

        /// <summary>
        /// Asynchronously counts the number of completed booking requests for a specific patient, identified by their unique ID.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient whose completed booking requests are to be counted.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in an integer value representing the total number of completed booking requests for the specified patient.
        /// </returns>
        /// <remarks>
        /// This method queries the Veezta database to count the number of bookings that have a status of 'Completed' and are associated with the given patient ID. This is useful for determining the number of successful service engagements a particular patient has had.
        /// </remarks>
        public async Task<int> GetNumberOfCompletedRequestByPatientId(string patientId)
        {
            var completedRequest = await _veeztaDbContext.Bookings.Where(b=> b.Status == BookingStatus.Completed && b.PatientId == patientId).CountAsync();
            return completedRequest;

        }

        /// <summary>
        /// Asynchronously updates the details of an existing coupon in the database.
        /// </summary>
        /// <param name="coupon">The coupon object with updated information to be saved in the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the updated <see cref="Coupon"/> object after the changes have been saved to the database.
        /// </returns>
        /// <remarks>
        public async Task<Coupon> UpdateCoupon(Coupon coupon)
        {
            _veeztaDbContext.Coupons.Update(coupon);
            await _veeztaDbContext.SaveChangesAsync();
            return coupon;
        }
    }
}

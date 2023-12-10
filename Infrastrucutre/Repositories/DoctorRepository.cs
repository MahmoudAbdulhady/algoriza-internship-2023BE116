using Domain.DTOS;
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
    public class DoctorRepository : IDoctorRepository
    {
        private readonly VeeztaDbContext _veeztaDbContext;

        public DoctorRepository(VeeztaDbContext veeztaDbContext)
        {
            _veeztaDbContext = veeztaDbContext;
        }

        /// <summary>
        /// Asynchronously adds a new doctor's appointment to the database.
        /// </summary>
        /// <param name="appointment">The appointment object to be added to the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the added <see cref="Appointment"/> object after it has been saved to the database.
        /// </returns>
        /// <remarks>
        /// This method adds the provided appointment object to the Veezta database context and saves the changes asynchronously. The same appointment object is returned after it's added to the database, which includes any database-generated fields like an auto-incremented ID.
        /// </remarks>
        public async Task<Appointement> AddDoctorAppointment(Appointement appointement)
        {
            _veeztaDbContext.Appointments.AddAsync(appointement);
            await _veeztaDbContext.SaveChangesAsync();
            return appointement;
        }

        /// <summary>
        /// Asynchronously adds a new time appointment to the database.
        /// </summary>
        /// <param name="time">The time appointment object to be added to the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the added <see cref="Time"/> object after it has been saved to the database.
        /// </returns>
        /// <remarks>
        /// Despite the name of the method, this implementation adds a new time appointment object to the Veezta database context and saves the changes asynchronously. It returns the same object after it's added to the database. If the intention is to update an existing time appointment, the method implementation needs to be revised.
        /// </remarks>
        public async Task<Time> UpdateTimeAppointment(Time time)
        {
            _veeztaDbContext.Times.AddAsync(time);
            await _veeztaDbContext.SaveChangesAsync();
            return time;
        }

        /// <summary>
        /// Asynchronously adds a new appointment to the database.
        /// </summary>
        /// <param name="appointment">The appointment object to be added to the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the added <see cref="Appointment"/> object after it has been saved to the database.
        /// </returns>
        /// <remarks>
        /// Despite the name of the method, this implementation adds a new appointment object to the Veezta database context and saves the changes asynchronously. It returns the same object after it's added to the database. If the intention is to update an existing appointment, the method implementation needs to be revised.
        /// </remarks>
        public async Task<Appointement> UpdateTimeAppointment(Appointement appointement)
        {
            _veeztaDbContext.Appointments.AddAsync(appointement);
            await _veeztaDbContext.SaveChangesAsync();
            return appointement;
        }

        /// <summary>
        /// Asynchronously retrieves a paginated list of appointments for a specific doctor from the database, with optional search functionality.
        /// </summary>
        /// <param name="request">A data transfer object containing pagination and search criteria.</param>
        /// <param name="doctorId">The unique identifier of the doctor whose appointments are to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a tuple. The first item is an <see cref="IEnumerable{Appointment}"/> representing a paginated list of appointments for the specified doctor, and the second item is an integer representing the total count of appointments matching the search and filter criteria.
        /// </returns>
        /// <remarks>
        /// This method filters appointments by the given doctor ID and allows for searching by doctor's full name or appointment times. The search is case-insensitive and includes partial matches. Pagination is applied after the search filter to provide a subset of results based on the specified page number and size. Related data from Booking, Doctor, Patient, and Times tables are included in the query.
        /// </remarks>
        public async Task<(IEnumerable<Appointement>, int)> GetDoctorApptAsync(PaginationAndSearchDTO request , int doctorId)
        {

            var query = _veeztaDbContext.Appointments
                .Include(b=> b.Booking) // Booking Table 
                //./*Include(d=> d.Doctor) // Doctor Table*/
                .Include(u=> u.Booking.Patient) // Identity Table
                .Include(t=> t.Times) // Times Table
                .Where(a=> a.DoctorId == doctorId && a.Booking != null)
                .AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm;

                query = query.Where(u =>
                    u.Doctor.User.FullName.Contains(searchTerm) ||
                    u.Times.Any(t => EF.Functions.Like(t.StartTime, $"%{searchTerm}%")) ||
                    u.Times.Any(t => EF.Functions.Like(t.EndTime, $"%{searchTerm}%")));
                    
            }
     
            int totalRecords = await query.CountAsync();
            var appointements = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                      .Take(request.PageSize)
                                      .ToListAsync();

            return (appointements, totalRecords);

        }

        /// <summary>
        /// Asynchronously updates the price for a specific doctor in the database.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose price is to be updated.</param>
        /// <param name="newPrice">The new price to be set for the doctor.</param>
        /// <remarks>
        /// This method first finds the doctor by their ID. If the doctor is found, it updates their price with the new value and saves the changes to the Veezta database context asynchronously. If no doctor is found with the given ID, no action is performed.
        /// </remarks>
        public async Task UpdateDoctorPrice(int doctorId, int newPrice)
        {
            var doctor = await _veeztaDbContext.Doctors.FindAsync(doctorId);
            if(doctor != null)
            {
                doctor.Price = newPrice;
                _veeztaDbContext.Doctors.Update(doctor);
                await _veeztaDbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Asynchronously removes a specific appointment time from the database.
        /// </summary>
        /// <param name="appointment">The appointment time object to be removed from the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the removed <see cref="Time"/> object after the changes have been saved to the database.
        /// </returns>
        /// <remarks>
        /// This method removes the specified appointment time from the Veezta database context and saves the changes asynchronously. The method returns the same time object after it is removed from the database. It's important to ensure that the passed appointment time object exists in the database before calling this method.
        /// </remarks>
        public async Task<Time> DeleteAppointmentTime(Time appointment)
        {
          
                _veeztaDbContext.Times.Remove(appointment);
                await _veeztaDbContext.SaveChangesAsync();
                return appointment;
          
           
        }

        /// <summary>
        /// Asynchronously searches for and retrieves an appointment time from the database by a specific appointment ID.
        /// </summary>
        /// <param name="appointmentId">The unique identifier of the appointment for which the time is to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="Time"/> object representing the appointment time associated with the specified appointment ID, or null if no matching time is found.
        /// </returns>
        /// <remarks>
        /// This method queries the Veezta database context for an appointment time that matches the provided appointment ID. It includes related details from the Appointment, Doctor, and User (Identity) tables. If no matching appointment time is found in the database, the method returns null. This is useful for retrieving specific appointment time details based on its associated appointment ID.
        /// </remarks>
        public async Task<Time> FindAppointmentByAppointmentId(int appoinmtmentId)
        {
           return  await  _veeztaDbContext.Times
                .Include(a=>a.Appointement) // Appointment Table
                .Include(d=> d.Appointement.Doctor) // Doctor Table
                .Include(u=>u.Appointement.Doctor.User) // Identity Table 
                .FirstOrDefaultAsync(time => time.AppointmentId == appoinmtmentId);
        }

        /// <summary>
        /// Asynchronously searches for and retrieves a booking from the database by a specific appointment ID.
        /// </summary>
        /// <param name="appointmentId">The unique identifier of the appointment for which the booking is to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="Booking"/> object representing the booking associated with the specified appointment ID, or null if no matching booking is found.
        /// </returns>
        /// <remarks>
        /// This method queries the Veezta database context for a booking that matches the provided appointment ID. It includes related details from the Times (within Appointment) and Appointment tables. If no matching booking is found in the database, the method returns null. This is useful for retrieving specific booking details based on its associated appointment ID.
        /// </remarks>
        public async Task<Booking> FindBookingByAppointmentId(int appointmentId)
        {
            var booking = await _veeztaDbContext.Bookings.Include(t=> t.Appointement.Times).Include(a=>a.Appointement).FirstOrDefaultAsync(b => b.AppointmentId == appointmentId);
            return booking;
        }

        /// <summary>
        /// Asynchronously updates the details of an existing appointment time in the database.
        /// </summary>
        /// <param name="timeEntity">The appointment time entity with updated information to be saved in the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in the updated <see cref="Time"/> object after the changes have been saved to the database.
        /// </returns>
        /// <remarks>
        /// This method marks the appointment time entity as modified in the Veezta database context and saves the changes asynchronously. The updated time entity, including any changes to its fields, is then returned. This method is useful for modifying existing appointment time details in the database.
        /// </remarks>
        public async Task<Time> DoctorAppointmentUpdateAsync(Time timeEntity)
        {
            _veeztaDbContext.Entry(timeEntity).State = EntityState.Modified;
            await _veeztaDbContext.SaveChangesAsync();
            return timeEntity;
        }

        /// <summary>
        /// Asynchronously updates the status of a booking to 'Completed' based on the provided booking ID.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking to be updated.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a boolean value. Returns true if the booking status is successfully updated to 'Completed', or false if the booking is not found.
        /// </returns>
        /// <remarks>
        /// This method first finds the booking by its ID. If the booking is found, it updates the status to 'Completed' and saves the changes to the Veezta database context asynchronously. The method returns true if the update is successful, or false if no booking with the given ID is found.
        /// </remarks>
        public async Task<bool> ConfirmCheckup(int bookingId)
        {
            var booking = await _veeztaDbContext.Bookings.FindAsync(bookingId);
            if(booking != null)
            {
                booking.Status = BookingStatus.Completed;
                await _veeztaDbContext.SaveChangesAsync();
                return true;
            }
            return false;
            
        }

       

    }
}

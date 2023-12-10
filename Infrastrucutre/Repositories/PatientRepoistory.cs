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
    public class PatientRepoistory : IPatientRepository
    {
        private readonly VeeztaDbContext _veeztaDbContext;
        private readonly IDoctorRepository _doctorRepository;

        public PatientRepoistory(VeeztaDbContext veeztaDbContext, IDoctorRepository doctorRepository)
        {
            _veeztaDbContext = veeztaDbContext;
            _doctorRepository = doctorRepository;
        }


        /// <summary>
        /// Asynchronously adds a new booking to the database.
        /// </summary>
        /// <param name="booking">The booking object to add to the database.</param>
        /// <remarks>
        /// This method adds the specified booking object to the database and saves the changes asynchronously.
        /// It assumes that the 'booking' parameter is not null and the database context (_veeztaDbContext) is properly configured.
        /// </remarks>
        public async Task AddBookingAsync(Booking booking)
        {
            _veeztaDbContext.Bookings.Add(booking);
            await _veeztaDbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Asynchronously cancels an appointment based on the provided booking ID.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to be canceled.</param>
        /// <returns>
        /// A boolean value indicating whether the appointment was successfully canceled. 
        /// Returns true if the cancellation is successful, otherwise false.
        /// </returns>
        /// <remarks>
        /// This method attempts to find a booking with the given ID. If found and the booking status is 'Pending',
        /// it sets the status to 'Canceled' and saves the changes to the database asynchronously. 
        /// If the booking is not in a 'Pending' state, it throws an exception indicating that the appointment cannot be canceled.
        /// </remarks>
        public async Task<bool> CancelAppointment(int bookingId)
        {
            var booking = await _veeztaDbContext.Bookings.FindAsync(bookingId);
            if(booking == null)
            {
                throw new Exception($"No Appointment with ID: { bookingId } is found");
            }
            if (booking != null)
            {
                if(booking.Status == BookingStatus.Pending)
                {
                    booking.Status = BookingStatus.Canceled;
                    await _veeztaDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception($"Appointnemt Id {bookingId} can't be cancled since its a Completed Appointment or Pending Appointment");
                }
            }
            return true;
        }

        /// <summary>
        /// Asynchronously creates a new booking and adds it to the database.
        /// </summary>
        /// <param name="booking">The booking object to be created and added to the database.</param>
        /// <remarks>
        /// This method adds the specified booking object to the database and saves the changes asynchronously. 
        /// It assumes that the 'booking' parameter is not null and that the database context (_veeztaDbContext) is properly configured.
        /// </remarks>
        public async Task CreateBookingAsync(Booking booking)
        {
            _veeztaDbContext.Bookings.Add(booking);
            await _veeztaDbContext.SaveChangesAsync();
        }



        /// <summary>
        /// Asynchronously finds and retrieves a booking by its unique ID.
        /// </summary>
        /// <param name="bookingId">The unique identifier of the booking to be retrieved.</param>
        /// <returns>
        /// The booking object with the specified ID if found, or null if no booking matches the ID.
        /// </returns>
        /// <remarks>
        /// This method searches for a booking in the database based on the provided 'bookingId'.
        /// If a matching booking is found, it is returned as the result; otherwise, null is returned.
        /// </remarks>
        public async Task<Booking> FindyBookingById(int bookingId)
        {
            var booking = await _veeztaDbContext.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            return booking;
        }

        /// <summary>
        /// Asynchronously checks if there is any booking associated with a specific appointment ID.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment to check for associated bookings.</param>
        /// <returns>
        /// A boolean value indicating whether a booking is found for the specified appointment ID. 
        /// Returns true if a booking exists, otherwise false.
        /// </returns>
        /// <remarks>
        /// This method queries the database to determine if there is any booking that is associated with the provided 'appointmentId'.
        /// It returns true if at least one booking is found with the specified appointment ID; otherwise, it returns false.
        /// </remarks>
        public async Task<bool> FindyBookingByAppoitmentId(int appointmentId)
        {
            var booking = await _veeztaDbContext.Bookings.AnyAsync(b => b.AppointmentId == appointmentId);
            return booking;
        }

        /// <summary>
        /// Asynchronously retrieves a paginated list of doctor appointments based on search criteria.
        /// </summary>
        /// <param name="request">A DTO (Data Transfer Object) containing pagination and search parameters.</param>
        /// <returns>
        /// A tuple containing an enumerable collection of appointments (Time objects) and the total count of appointments.
        /// </returns>
        /// <remarks>
        /// This method queries the database to retrieve a list of doctor appointments. It includes various related entities,
        /// such as doctor information and specialization, for each appointment.
        /// 
        /// - The 'request' parameter allows specifying search criteria (search term) and pagination settings (page number and page size).
        /// - If a search term is provided, the method filters appointments based on doctor's full name, email, or specialization containing the search term.
        /// - Pagination is applied to the query, and the method returns a subset of appointments based on the specified page number and page size.
        /// - The total count of all matching appointments is also returned to facilitate pagination controls.
        /// </remarks>
        /// <param name="request">A DTO (Data Transfer Object) containing pagination and search parameters.</param>
        /// <returns>
        /// A tuple containing an enumerable collection of appointments (Time objects) and the total count of appointments.
        /// </returns>
        public async Task<(IEnumerable<Time> , int totalCounts)> GetDoctorApptAsync(PaginationAndSearchDTO request)
        {

            var query = _veeztaDbContext.Times
                .Include(a => a.Appointement)
                .Include(a => a.Appointement.Doctor)
                .Include(a => a.Appointement.Doctor.User)
                .Include(a => a.Appointement.Doctor.Specialization)
                .AsQueryable();


            // Search functionality
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(d => d.Appointement.Doctor.User.FullName.Contains(request.SearchTerm) ||
                                    d.Appointement.Doctor.User.Email.Contains(request.SearchTerm) ||
                                    d.Appointement.Doctor.Specialization.SpecializationName.Contains(request.SearchTerm));
            }

            // Pagination
            int totalCount = await query.CountAsync();
            var doctors = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                                     .ToListAsync();

            return (doctors, totalCount);
        }

        /// <summary>
        /// Asynchronously retrieves a collection of bookings made by a patient, including related information.
        /// </summary>
        /// <returns>
        /// An enumerable collection of bookings made by a patient, along with associated appointment, doctor, user, and specialization details.
        /// </returns>
        /// <remarks>
        /// This method queries the database to retrieve a collection of bookings made by a patient. It includes various related entities,
        /// such as the appointment, doctor, user (identity), and specialization, for each booking.
        /// 
        /// The method fetches all the necessary related information to provide comprehensive details about each booking.
        /// </remarks>
        public async Task<IEnumerable<Booking>> GetPatientBookings()
        {
            return await _veeztaDbContext.Bookings
                    .Include(t=> t.Appointement.Times)
                .Include(d => d.Appointement.Doctor) // Doctor Table
                .Include(a => a.Appointement) // Appointment Table
                .Include(u=> u.Appointement.Doctor.User) // Identity Table
                .Include(s=> s.Appointement.Doctor.Specialization) //Specalization Table
                .ToListAsync();
        }
    }

    

}

        


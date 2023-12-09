using Domain.DTOS;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucutre.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly VeeztaDbContext _veeztaDbContext;
        private readonly UserManager<CustomUser> _userManager;
        
        public AdminRepository(UserManager<CustomUser> userManager, VeeztaDbContext veeztaDbContext)
        {
            _veeztaDbContext = veeztaDbContext;
            _userManager = userManager;
        }

        /// <summary>
        /// Asynchronously adds a new doctor to the database.
        /// </summary>
        /// <param name="doctor">The doctor object to be added to the database.</param>
        /// <remarks>
        /// This method adds the provided doctor object to the Veezta database context and saves the changes asynchronously.
        /// </remarks>
        public async Task CreateNewDoctorAsync(Doctor doctor)
        {
            await _veeztaDbContext.Doctors.AddAsync(doctor);
            await _veeztaDbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Returns the total number of doctors in the database.
        /// </summary>
        /// <returns>
        /// An integer representing the count of doctor records in the Veezta database.
        /// </returns>
        /// <remarks>
        /// This method counts the number of doctor entries in the Veezta database and returns the result. It does not filter or modify the data in any way.
        /// </remarks>
        public int NumOfDoctors()
        {
            return _veeztaDbContext.Doctors.Count();
        }

        /// <summary>
        /// Asynchronously retrieves a doctor from the database by their unique identifier.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> that, when completed, results in a <see cref="Doctor"/> object representing the doctor with the specified ID. Returns null if no matching doctor is found.
        /// </returns>
        /// <remarks>
        /// This method searches the Veezta database for a doctor with the given ID. It includes related data from the User, Specialization, and Appointments entities. If no doctor is found with the given ID, the method returns null.
        /// </remarks>
        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            return await _veeztaDbContext.Doctors.Include(d => d.User).Include(d => d.Specialization).Include(b=> b.Appointements).FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }

        /// <summary>
        /// Asynchronously checks if there are any bookings for a specified doctor.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose bookings are to be checked.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a boolean value. Returns true if there are any bookings for the specified doctor, false otherwise.
        /// </returns>
        /// <remarks>
        /// This method searches the Veezta database for bookings related to the given doctor ID. It includes the Appointment data in the search and checks if there are any matching bookings. This is useful for determining if a doctor has current appointments.
        /// </remarks>
        public async Task<bool>GetBookingByDoctorId(int doctorId)
        {
            return await _veeztaDbContext.Bookings
                .Include(a => a.Appointement).Where(a => a.Appointement.DoctorId == doctorId).AnyAsync();
        }


        /// <summary>
        /// Asynchronously removes a doctor from the database.
        /// </summary>
        /// <param name="doctor">The doctor object to be removed from the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a boolean value. Returns true after successfully removing the doctor from the database.
        /// </returns>
        /// <remarks>
        /// This method removes the specified doctor object from the Veezta database context and saves the changes. It always returns true to indicate the completion of the delete operation, but it doesn't handle cases where the specified doctor does not exist in the database.
        /// </remarks>
        public async Task<bool> DeleteDoctorAsync(Doctor doctor)
        {
            _veeztaDbContext.Doctors.Remove(doctor);
            await _veeztaDbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Asynchronously updates the details of an existing doctor in the database.
        /// </summary>
        /// <param name="doctor">The doctor object with updated information to be saved in the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a boolean value. Returns true after successfully updating the doctor's details in the database.
        /// </returns>
        /// <remarks>
        /// This method marks the doctor entity as modified in the Veezta database context and saves the changes. It always returns true to indicate the completion of the update operation. Note that this method assumes the doctor object provided already exists in the database and has a valid state.
        /// </remarks>
        public async Task<bool> DoctorUpdateAsync(Doctor doctor)
        {
            _veeztaDbContext.Entry(doctor).State = EntityState.Modified;
            await _veeztaDbContext.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// Asynchronously retrieves a paginated list of doctors from the database, with optional search functionality.
        /// </summary>
        /// <param name="request">A data transfer object containing pagination and search criteria.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a tuple. The first item is an <see cref="IEnumerable{Doctor}"/> representing a paginated list of doctors, and the second item is an integer representing the total count of doctors matching the search criteria.
        /// </returns>
        /// <remarks>
        /// This method allows searching for doctors by their full name, email, or specialization name. The search is case-insensitive and includes partial matches. Pagination is applied after filtering to provide a subset of results based on the specified page number and size. This method includes related data from the User and Specialization entities.
        /// </remarks>
        public async Task<(IEnumerable<Doctor>, int TotalCount)> GetAllDoctorsAsync(PaginationAndSearchDTO request)
        {
            var query = _veeztaDbContext.Doctors.AsQueryable();
            

            // Search functionality
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(d => d.User.FullName.Contains(request.SearchTerm) ||
                                    d.User.Email.Contains(request.SearchTerm) ||
                                    d.Specialization.SpecializationName.Contains(request.SearchTerm));
            }

            // Pagination
            int totalCount = await query.CountAsync();
            var doctors = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
                                     .Include(d => d.User)  // Include Identity Table (Custom User)
                                     .Include(d => d.Specialization) // Include Specialization
                                     .ToListAsync();

            return (doctors, totalCount);
        }


        /// <summary>
        /// Asynchronously updates the details of an existing doctor in the database.
        /// </summary>
        /// <param name="doctor">The doctor object with updated information to be saved in the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a boolean value. Returns true after successfully updating the doctor's details in the database.
        /// </returns>
        /// <remarks>
        /// This method marks the doctor entity as modified in the Veezta database context and saves the changes. It always returns true to indicate the completion of the update operation. Note that this method assumes the doctor object provided already exists in the database and has a valid state.
        /// </remarks>
        public async Task<int> NumberOfPatients()
        {
            return await _userManager.Users.CountAsync(u => u.AccountRole == AccountRole.Patient);
        }

        /// <summary>
        /// Asynchronously retrieves a paginated list of all patients from the database, with optional search functionality.
        /// </summary>
        /// <param name="request">A data transfer object containing pagination and search criteria.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a tuple. The first item is an <see cref="IEnumerable{CustomUser}"/> representing a paginated list of patients, and the second item is an integer representing the total count of patients matching the search criteria.
        /// </returns>
        /// <remarks>
        /// This method filters users by the 'Patient' role and allows for searching by first name, last name, full name, email, or phone number. The search is case-insensitive and includes partial matches. Pagination is applied after the search filter to provide a subset of results based on the specified page number and size.
        /// </remarks>
        public async Task<(IEnumerable<CustomUser>, int)> GetAllPatientsAsync(PaginationAndSearchDTO request)
        {


            var query = _userManager.Users.Where(u => u.AccountRole == AccountRole.Patient).AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(
                    u =>u.FirstName.Contains(request.SearchTerm)||
                    u.LastName.Contains(request.SearchTerm) ||
                    u.FullName.Contains(request.SearchTerm) || 
                    u.Email.Contains(request.SearchTerm) ||
                    u.PhoneNumber.Contains(request.SearchTerm));
            }

            var totalItems = await query.CountAsync();
            var users = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            return (users, totalItems);
        }

        /// <summary>
        /// Asynchronously retrieves a patient by their unique identifier.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="CustomUser"/> object representing the patient. Returns null if the user is not found or the user is not assigned the 'Patient' role.
        /// </returns>
        /// <remarks>
        /// This method first finds the user by their ID using the user manager. If the user is found, it then checks if the user has been assigned the 'Patient' role. Only users with the 'Patient' role are returned; otherwise, null is returned. This ensures that only valid patient accounts are retrieved.
        /// </remarks>
        public async Task<CustomUser> GetPatientById(string patientId)
        {
            var user = await _userManager.FindByIdAsync(patientId);
            if(user !=null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(AccountRole.Patient.ToString()))
                    
                 {
                    return user;
                }
            }
            return user;
        }

        /// <summary>
        /// Asynchronously retrieves all bookings that have been marked as completed, including details of the associated appointments, doctors, and their specializations.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="List{Booking}"/> containing all completed bookings.
        /// </returns>
        /// <remarks>
        /// This method fetches bookings from the database that have a status of 'Completed'. It includes related appointment details, doctor information, and specialization data. Note that this method currently does not limit the results to the top five specializations.
        /// </remarks>
        public async Task<List<Booking>> GetTopFiveSpecalizationsAsync()
        {
            var completedBooking = await _veeztaDbContext.Bookings.Include(a => a.Appointement.Doctor.Specialization)
                .Where(b => b.Status == BookingStatus.Completed)
                .ToListAsync();
            return completedBooking;
        }

        /// <summary>
        /// Asynchronously retrieves all bookings that have been marked as completed, including details of the associated doctors and their specializations.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="List{Booking}"/> containing all completed bookings with details of the doctors involved.
        /// </returns>
        /// <remarks>
        /// This method fetches bookings from the database that have a status of 'Completed'. It includes related details of doctors and their specializations. Note that this method currently does not limit or rank the results based on the top ten doctors, contrary to what the method name suggests.
        /// </remarks>
        public async Task<List<Booking>> GetTopTenDoctors()
        {
            var completedRequests = await _veeztaDbContext.Bookings
                .Include(a => a.Appointement.Doctor.User).Include(a => a.Appointement.Doctor.Specialization)
                .Where(b => b.Status == BookingStatus.Completed)
                .ToListAsync();

            return completedRequests;
        }


        /// <summary>
        /// Asynchronously calculates and retrieves the counts of bookings in different statuses (Pending, Completed, Canceled).
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> resulting in a <see cref="RequestsDTO"/> object containing the number of bookings for each status: pending, completed, and canceled.
        /// </returns>
        /// <remarks>
        /// This method fetches all booking statuses from the database and then calculates the count of bookings in each status: Pending, Completed, and Canceled. The counts are then encapsulated in a RequestsDTO object and returned. This method is useful for getting an overview of the distribution of booking statuses.
        /// </remarks>
        public async Task<RequestsDTO> GetNumberOfRequests()
        {
            var allRequestsStatuses = await _veeztaDbContext.Bookings.Select(b => b.Status).ToListAsync();

            var statusCounts = new RequestsDTO
            {
                NumOfPendingRequest = allRequestsStatuses.Count(s => s == BookingStatus.Pending),
                NumOfCompletedRequest = allRequestsStatuses.Count(s => s == BookingStatus.Completed),
                NumOfCanceledRequest = allRequestsStatuses.Count(s => s == BookingStatus.Canceled)

            };

            return statusCounts;
        }


        /// <summary>
        /// Asynchronously calculates the number of doctors who were added to the database in the last 24 hours.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> resulting in an integer value representing the count of doctors added to the database in the last 24 hours.
        /// </returns>
        /// <remarks>
        /// This method calculates the current UTC date and time, subtracts 24 hours to determine the starting point, and then counts the number of doctors in the database who were created within this 24-hour window. This is useful for monitoring recent additions to the list of doctors.
        /// </remarks>
        public async Task<int> GetNumberOfDoctorsAddedLast24HoursAsync()
        {
            var currentDate = DateTime.UtcNow;
            var date24HoursAgo = currentDate.AddHours(-24);

            return await _veeztaDbContext.Doctors
                .CountAsync(d => d.CreatedDate >= date24HoursAgo && d.CreatedDate <= currentDate);
        }

        /// <summary>
        /// Asynchronously retrieves the first appointment associated with a specified doctor.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose appointment is to be retrieved.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in an <see cref="Appointment"/> object representing the first appointment found for the specified doctor, or null if no appointment is found.
        /// </returns>
        /// <remarks>
        /// This method searches for appointments in the database that are associated with the given doctor ID and returns the first one it finds. If no appointment is associated with the specified doctor, the method returns null. This method is useful for quickly retrieving an upcoming or recent appointment for a specific doctor.
        /// </remarks>
        public async Task<Appointement> GetAppointmentByDoctorId(int doctorId)
        {
         var doctorAppointment =  await _veeztaDbContext.Appointments.Where(a => a.DoctorId == doctorId).FirstOrDefaultAsync();
            return doctorAppointment;
        }

        /// <summary>
        /// Asynchronously deletes a specified doctor's appointment from the database.
        /// </summary>
        /// <param name="doctorAppointment">The appointment object associated with a doctor to be removed from the database.</param>
        /// <returns>
        /// A <see cref="Task"/> resulting in a boolean value. Returns true after successfully removing the appointment from the database.
        /// </returns>
        /// <remarks>
        /// This method removes the specified doctor's appointment from the Veezta database context and saves the changes. It always returns true to indicate the completion of the delete operation, but it doesn't handle cases where the specified appointment does not exist in the database.
        /// </remarks>
        public async Task<bool> DeleteDoctorAppointmentAsync(Appointement doctrorAppointment)
        {
             _veeztaDbContext.Appointments.Remove(doctrorAppointment);
            await _veeztaDbContext.SaveChangesAsync();
            return true;
        }
    }
}


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

        public async Task CreateNewDoctorAsync(Doctor doctor)
        {
            await _veeztaDbContext.Doctors.AddAsync(doctor);
            await _veeztaDbContext.SaveChangesAsync();
        }
        public int NumOfDoctors()
        {
            return _veeztaDbContext.Doctors.Count();
        }
        public async Task<Doctor> GetDoctorByIdAsync(int doctorId)
        {
            return await _veeztaDbContext.Doctors.Include(d => d.User).Include(d => d.Specialization).FirstOrDefaultAsync(d => d.DoctorId == doctorId);
        }

        public async Task<bool> DeleteDoctorAsync(Doctor doctor)
        {
            _veeztaDbContext.Doctors.Remove(doctor);
            await _veeztaDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DoctorUpdateAsync(Doctor doctor)
        {
            _veeztaDbContext.Entry(doctor).State = EntityState.Modified;
            await _veeztaDbContext.SaveChangesAsync();
            return true;
        }


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

        public async Task<int> NumberOfPatients()
        {
            return await _userManager.Users.CountAsync(u => u.AccountRole == AccountRole.Patient);
        }

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

        public async Task<List<Booking>> GetTopFiveSpecalizationsAsync()
        {
            var completedBooking = await _veeztaDbContext.Bookings.Include(a => a.Appointement.Doctor.Specialization)
                .Where(b => b.Status == BookingStatus.Completed)
                .ToListAsync();
            return completedBooking;
        }

        public async Task<List<Booking>> GetTopTenDoctors()
        {
            var completedRequests = await _veeztaDbContext.Bookings
                .Include(a => a.Appointement.Doctor.User).Include(a => a.Appointement.Doctor.Specialization)
                .Where(b => b.Status == BookingStatus.Completed)
                .ToListAsync();

            return completedRequests;
        }

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
        public async Task<int> GetNumberOfDoctorsAddedLast24HoursAsync()
        {
            var currentDate = DateTime.UtcNow;
            var date24HoursAgo = currentDate.AddHours(-24);

            return await _veeztaDbContext.Doctors
                .CountAsync(d => d.CreatedDate >= date24HoursAgo && d.CreatedDate <= currentDate);
        }
    }
}


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

        public async Task<Appointement> AddDoctorAppointment(Appointement appointement)
        {
            _veeztaDbContext.Appointments.AddAsync(appointement);
            await _veeztaDbContext.SaveChangesAsync();
            return appointement;
        }


        public async Task<Time> UpdateTimeAppointment(Time time)
        {
            _veeztaDbContext.Times.AddAsync(time);
            await _veeztaDbContext.SaveChangesAsync();
            return time;
        }

        public async Task<Appointement> UpdateTimeAppointment(Appointement appointement)
        {
            _veeztaDbContext.Appointments.AddAsync(appointement);
            await _veeztaDbContext.SaveChangesAsync();
            return appointement;
        }

        public async Task<(IEnumerable<Appointement>, int)> GetDoctorApptAsync(PaginationAndSearchDTO request , int doctorId)
        {

            var query = _veeztaDbContext.Appointments
                .Include(b=> b.Booking) // Booking Identity 
                .Include(d=> d.Doctor) // Doctor Table
                .Include(u=> u.Booking.Patient) // Identity Table
                .Include(t=> t.Times) // Times Table
                .Where(a=> a.DoctorId == doctorId)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var searchTerm = request.SearchTerm;

                query = query.Where(u =>
                    u.Doctor.User.FullName.Contains(searchTerm) ||
                    u.Times.Any(t => EF.Functions.Like(t.StartTime, $"%{searchTerm}%")) ||
                    u.Times.Any(t => EF.Functions.Like(t.EndTime, $"%{searchTerm}%")));
                    
            }
     
            int totalRecords = await query.CountAsync();
            var bookings = await query.Skip((request.PageNumber - 1) * request.PageSize)
                                      .Take(request.PageSize)
                                      .ToListAsync();

            return (bookings, totalRecords);








            //var query = _veeztaDbContext.Appointments
            //  .Include(u => u.Doctor.User) // Identity 
            //  .Include(t => t.Times) // Times
            //  .Include(b=> b.Booking) // Booking Table 
            //  .AsQueryable();


            //if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            //{
            //    var searchTerm = request.SearchTerm.ToLower();

            //    query = query.Where(u =>
            //        u.Doctor.User.FullName.ToLower().Contains(searchTerm) ||
            //        u.Times.Any(t => t.StartTime.ToLower().Contains(searchTerm)) ||
            //        u.Times.Any(t => t.EndTime.ToLower().Contains(searchTerm)));
            //}

            //int totalPages = await query.CountAsync();
            //var doctorBookings = await query.Skip((request.PageNumber - 1) * request.PageSize)
            //                                .Take(request.PageSize)
            //                                .ToListAsync();
            //return (doctorBookings, totalPages);







            //// Start with a queryable that can be modified based on conditions

            //// Apply search term if present
            //if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            //{
            //    query = query.Where(
            //        u => u.Doctor.User.FullName.ToLower().Contains(request.SearchTerm) ||
            //        u.Times.Any(u => u.StartTime.Contains(request.SearchTerm)) ||
            //        u.Times.Any(u => u.EndTime.Contains(request.SearchTerm)) ||
            //        u.Days.(request.SearchTerm));
            //}

            //int totalPages = await query.CountAsync();
            //var doctorbookings = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize)
            //                            .ToListAsync();
            //return (doctorbookings, totalPages);
        }

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

        public async Task<Time> DeleteAppointmentTime(Time appointment)
        {
          
                _veeztaDbContext.Times.Remove(appointment);
                await _veeztaDbContext.SaveChangesAsync();
                return appointment;
          
           
        }

        public async Task<Time> FindAppointmentByAppointmentId(int appoinmtmentId)
        {
           return  await  _veeztaDbContext.Times
                .Include(a=>a.Appointement) // Appointment Table
                .Include(d=> d.Appointement.Doctor) // Doctor Table
                .Include(u=>u.Appointement.Doctor.User) // Identity Table 
                .FirstOrDefaultAsync(time => time.AppointmentId == appoinmtmentId);
        }


        public async Task<Booking> FindBookingByAppointmentId(int appointmentId)
        {
            var booking = await _veeztaDbContext.Bookings.Include(t=> t.Appointement.Times).Include(a=>a.Appointement).FirstOrDefaultAsync(b => b.AppointmentId == appointmentId);
            return booking;
        }
        public async Task<Time> DoctorAppointmentUpdateAsync(Time timeEntity)
        {
            _veeztaDbContext.Entry(timeEntity).State = EntityState.Modified;
            await _veeztaDbContext.SaveChangesAsync();
            return timeEntity;
        }

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

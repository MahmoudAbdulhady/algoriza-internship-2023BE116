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

        public async Task<(IEnumerable<Booking>, int)> GetDoctorApptAsync(PaginationAndSearchDTO request)
        {
            // Start with a queryable that can be modified based on conditions
            var query = _veeztaDbContext.Bookings
                .Include(u => u.Patient)
                .Include(a => a.Appointement)
                    .ThenInclude(a => a.Times)
                .AsQueryable();

            // Apply search term if present
            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                // Adjust this condition based on your search requirements
                query = query.Where(b =>
                    b.Appointement.Times.Any(t => t.StartTime.ToString().Contains(request.SearchTerm) ||
                                                  t.EndTime.ToString().Contains(request.SearchTerm)));
            }

            // Count total items matching the criteria
            var totalCount = await query.CountAsync();

            // Apply pagination
            var paginatedBookings = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return (paginatedBookings, totalCount);
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

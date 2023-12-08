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

        public async Task<Appointement> AddDoctorAppointmentAsync(Appointement appointement)
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

        public async Task<IEnumerable<Booking>> GetDoctorApptAsync()
        {
            return await _veeztaDbContext.Bookings
                .Include(a=> a.Time)   // Time Table 
                .Include(a => a.Appointement) // Doctor Table
                .Include(a => a.Patient) // Identity  Table
                .ToListAsync();
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

        public async Task<Time> DeleteAppointmentTime(int timeId)
        {
            var time = await _veeztaDbContext.Times.FindAsync(timeId);
            if (time != null)
            {
                _veeztaDbContext.Times.Remove(time);
                await _veeztaDbContext.SaveChangesAsync();
            }
            return time;
        }

        public async Task<Time> FindAppointmentTimeById(int timeId)
        {
           return  await  _veeztaDbContext.Times
                .Include(a=>a.Appointements)
                .Include(d=> d.Appointements.Doctor)
                .Include(u=>u.Appointements.Doctor.User)
                .Include(b=> b.Bookings)
                .FirstOrDefaultAsync(time => time.TimeId == timeId);
        }


        public async Task<Booking> FindBookingByTimeId(int timeId)
        {
            var booking = await _veeztaDbContext.Bookings.Include(t=> t.Time).FirstOrDefaultAsync(b => b.TimeId == timeId);
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

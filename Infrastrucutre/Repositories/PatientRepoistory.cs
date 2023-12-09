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

        public async Task AddBookingAsync(Booking booking)
        {
            _veeztaDbContext.Bookings.Add(booking);
            await _veeztaDbContext.SaveChangesAsync();
        }

        public async Task<bool> CancelAppointment(int bookingId)
        {
            var booking = await _veeztaDbContext.Bookings.FindAsync(bookingId);
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
                    throw new Exception($"Appointnemt Id {bookingId} can't be cancled since its a Completed Appointment");
                }
            }
            return true;
        }

        public async Task CreateBookingAsync(Booking booking)
        {
            _veeztaDbContext.Bookings.Add(booking);
            await _veeztaDbContext.SaveChangesAsync();
        }

        //TODO I need to review this section 
        public async Task<Booking> FindyBookingById(int bookingId)
        {
            var booking = await _veeztaDbContext.Bookings.FirstOrDefaultAsync(b => b.BookingId == bookingId);
            return booking;
        }
        public async Task<bool> FindyBookingByAppoitmentId(int appointmentId)
        {
            var booking = await _veeztaDbContext.Bookings.AnyAsync(b => b.AppointmentId == appointmentId);
            return booking;
        }

        public async Task<IEnumerable<Time>> GetDoctorApptAsync()
        {
            return await _veeztaDbContext.Times
                .Include(a => a.Appointement)   // Appointment Table 
                .Include(a => a.Appointement.Doctor) // Doctor Table
                .Include(a => a.Appointement.Doctor.User) // Identity  Table
                .Include(a => a.Appointement.Doctor.Specialization) // Specalization Table               
                .ToListAsync();
        }

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

        


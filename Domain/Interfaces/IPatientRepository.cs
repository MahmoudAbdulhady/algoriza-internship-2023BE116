using Domain.DTOS;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<(IEnumerable<Time>, int totalCounts)> GetDoctorApptAsync(PaginationAndSearchDTO request);
        Task CreateBookingAsync(Booking booking);
        Task<bool> CancelAppointment(int bookingId);
        Task<Booking>FindyBookingById (int bookingId);
        Task<IEnumerable<Booking>> GetPatientBookings();
        Task<bool> FindyBookingByAppoitmentId(int ppointmentId);


    }
}

using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Booking>> GetDoctorApptAsync();
        Task UpdateDoctorPrice(int doctorId, int newPrice);

        Task<Appointement> AddDoctorAppointmentAsync(Appointement appointement);
        Task<Time> UpdateTimeAppointment(Time time);
        Task<Time> FindAppointmentTimeById(int timeId);
        Task<Time> DeleteAppointmentTime(int timeId);
        Task<Time> DoctorAppointmentUpdateAsync(Time TimeEntity);
        Task<bool>ConfirmCheckup(int bookingId);
        Task<Booking> FindBookingByTimeId(int timeId);
    }
}

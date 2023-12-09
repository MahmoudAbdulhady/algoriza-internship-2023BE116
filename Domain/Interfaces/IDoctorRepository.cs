using Domain.DTOS;
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
        Task<(IEnumerable<Booking>, int)> GetDoctorApptAsync(PaginationAndSearchDTO request);
        Task UpdateDoctorPrice(int doctorId, int newPrice);

        Task<Appointement> AddDoctorAppointment(Appointement appointement);
        Task<Time> UpdateTimeAppointment(Time time);
        //Task<Appointement> UpdateTimeAppointment(DayTimes time);
        Task<Time> FindAppointmentByAppointmentId(int apppointmentId);
        Task<Time> DeleteAppointmentTime(Time appointment);
        Task<Time> DoctorAppointmentUpdateAsync(Time TimeEntity);
        Task<bool> ConfirmCheckup(int bookingId);
        Task<Booking> FindBookingByAppointmentId(int appointmentId);

    }
}

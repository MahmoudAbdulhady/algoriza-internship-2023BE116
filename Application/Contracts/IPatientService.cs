using Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IPatientService
    {
        Task<bool> RegisterPatientAsync(PatientRegisterDTO model);
        Task<bool> LoginPatientAsync(LoginDTO model);
        Task<IEnumerable<AppointmentDTO>> GetAppointmentsForDoctorAsync();
        Task<bool> CreateNewBooking(int timeId, string patientId, string? couponName = null);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<IEnumerable<PatientBookingDTO>> GetPatientSpecificBookingsAsync(string patientId);


    }
}

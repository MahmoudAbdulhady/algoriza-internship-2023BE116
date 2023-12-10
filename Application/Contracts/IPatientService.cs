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
        Task<(IEnumerable<AppointmentDTO>, int totalCount)> GetAppointmentsForDoctorAsync(PaginationAndSearchDTO request);
        Task<bool> CreateNewBookingAsync(int timeId, string patientId, string? couponName = null);
        Task<bool> CancelBookingAsync(int bookingId);
        Task<IEnumerable<PatientBookingDTO>> GetPatientSpecificBookingsAsync(string patientId);


    }
}

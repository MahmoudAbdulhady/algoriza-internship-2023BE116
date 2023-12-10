using Domain.DTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IDoctorService
    {
        Task<bool> DoctorLoginAsync(LoginDTO model);
        Task<(IEnumerable<DoctorBookingsDTO>, int totalCounts)> GetAppointmentsForDoctorAsync(int doctorId, PaginationAndSearchDTO request);
        Task<bool> AddDoctorAppointmentAsync(AddAppointmentDTO doctorDTO);
        Task<bool> DeleteTimeAppointmentAsync(int appointmentId);
        Task<bool> DoctorUpdateAppointmentAsync(int timeId, UpdateAppointmentDTO model);
        Task<bool> DoctorConfirmCheckUpAsync(int bookingId);
        


    }
}

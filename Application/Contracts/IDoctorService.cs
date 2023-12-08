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
        Task<IEnumerable<DoctorBookingsDTO>> GetAppointmentsForDoctorAsync(int doctorId);
        Task<bool> AddDoctorAppointment(AddAppointmentDTO doctorDTO);
        Task<bool> DeleteTimeAppointmentAsync(int timeId);
        Task<bool> DoctorUpdateAppointmentAsync(int timeId, UpdateAppointmentDTO model);
        Task<bool> DoctorConfirmCheckUp(int bookingId);
        


    }
}

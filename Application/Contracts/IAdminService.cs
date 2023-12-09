using Domain.DTOS;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IAdminService
    {
        Task<bool> AddDocotorAsync(DoctorRegisterDTO model);
        Task<DoctorDTO> GetDoctorByIdAsync(int doctorId);
        Task<bool> DeleteDoctorAsync(int doctorId);
        int GetTotalNumOfDoctors();
        Task<bool> DoctorUpdateAsync(int doctorId, DoctorUpdateDTO model);
        Task<(IEnumerable<DoctorDTO>, int TotalCount)> GetAllDoctorsAsync(PaginationAndSearchDTO request);
        Task<int> TotalNumberOfPatients();
        Task<(IEnumerable<PatientDTO>, int)> GetAllPatientsAsync(PaginationAndSearchDTO request);
        Task<PatientDTO> GetPatientByIdAsync(string patientId);
        Task<IEnumerable<TopFiveSpecalizationDTO>> GetTopSpecializationsAsync();
        Task<IEnumerable<TopTenDoctorDTO>> GetTopTenDoctors();
        Task<RequestsDTO> GetNumberOfRequestsAsync();
        Task<int> GetNumberOfDoctorsAddedLast24HoursAsync();

        Task SendEmailToDoctorAsync(string doctorId);

    }
}

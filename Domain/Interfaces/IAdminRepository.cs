using Domain.DTOS;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAdminRepository
    {
        //Doctor DashBoard (Admin Management)
        Task CreateNewDoctorAsync(Doctor doctor);
        Task<Doctor> GetDoctorByIdAsync(int doctorId);
        Task<bool> DeleteDoctorAsync(Doctor doctor);
        Task<bool> DoctorUpdateAsync(Doctor doctor);
        int NumOfDoctors();
        Task<(IEnumerable<Doctor>, int TotalCount)> GetAllDoctorsAsync(PaginationAndSearchDTO request);

        //Patient DashBoard (Admin Management) 
        Task<int> NumberOfPatients();

        //Task<(IEnumerable<PatientDTO>, int)> GetAllPatientAsync(string roleName, PaginationAndSearchDTO request);
        Task<(IEnumerable<CustomUser>, int)> GetAllPatientsAsync(PaginationAndSearchDTO request);
        Task<CustomUser>GetPatientById(string patientId);
        Task<List<Booking>> GetTopFiveSpecalizationsAsync();
        Task<List<Booking>> GetTopTenDoctors();
        Task<RequestsDTO> GetNumberOfRequests();

    }
}

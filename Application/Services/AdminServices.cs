
using Domain.DTOS;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Infrastrucutre;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdminServices
    {
        private readonly IAdminRepository _adminRepository;
        private readonly UserManager<CustomUser> _userManager;
        private readonly ISpecializationRepository _specilizationRepository;



        public AdminServices(IAdminRepository adminRepository, UserManager<CustomUser> userManager, ISpecializationRepository specilizationRepository)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
            _specilizationRepository = specilizationRepository;
        }

        public async Task<bool> AddDocotorAsync(DoctorRegisterDTO model)
        {

            var user = new CustomUser
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                FullName = model.FirstName + " " + model.LastName,
                PhoneNumber = model.PhoneNumber,
                DateOfBirth = DateTime.Parse(model.DateOfBirth),
                Gender = model.Gender,
                AccountRole = AccountRole.Doctor
            };
            var userResult = await _userManager.CreateAsync(user, model.Password);
            if (!userResult.Succeeded)
            {
                throw new Exception(userResult.Errors.ToString());
            }
            await _userManager.AddToRoleAsync(user, AccountRole.Doctor.ToString());


            var specialization = await _specilizationRepository.GetByNameAsync(model.Specialization);
            if (specialization == null)
            {
                // Handle or create the specialization
                specialization = new Specialization { SpecializationName = model.Specialization };
                await _specilizationRepository.AddAsync(specialization);
            }


            var doctor = new Doctor
            {
                UserId = user.Id,
                SpecializationId = specialization.SpecializationId
            };

            await _adminRepository.CreateNewDoctorAsync(doctor);
            return true;
        }

        public async Task<DoctorDTO> GetDoctorByIdAsync(int doctorId)
        {
            var doctor = await _adminRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                return null;
            }

            return new DoctorDTO
            {
                Email = doctor.User.Email,
                FullName = doctor.User.FullName,
                PhoneNumber = doctor.User.PhoneNumber,
                Specilization = doctor.Specialization.SpecializationName,
                Gender = doctor.User.Gender.ToString(),
                DateOfBirth = Convert.ToString(doctor.User.DateOfBirth),
                ImageUrl = doctor.User.ImageUrl
            };
        }

        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            var doctor = await _adminRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception($"No Doctor with This ID: {doctorId} was found, Check DoctorDB again");
            }
            await _adminRepository.DeleteDoctorAsync(doctor);

            var user = await _userManager.FindByIdAsync(doctor.UserId);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return false;
                }
            }
            return true;

        }

        public int GetTotalNumOfDoctors()
        {
            return _adminRepository.NumOfDoctors();
        }

        public async Task<bool> DoctorUpdateAsync(int doctorId, DoctorUpdateDTO model)
        {
            var doctor = await _adminRepository.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception($"No Doctor with This ID: {doctorId} was found, Check DoctorDB again");
            }

            //Updating Identity Table
            var user = await _userManager.FindByIdAsync(doctor.UserId);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
                user.ImageUrl = model.ImageUrl;
                user.PhoneNumber = model.PhoneNumber;
                user.Gender = model.Gender;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.FullName = model.FirstName + " " + model.LastName;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return false;
                }
            }

            //Updating Specailization Table
            var specailization = await _specilizationRepository.GetByNameAsync(model.Specilization);
            if (specailization != null)
            {
                doctor.SpecializationId = specailization.SpecializationId;
            }
            return await _adminRepository.DoctorUpdateAsync(doctor);
        }

        public async Task<(IEnumerable<DoctorDTO>, int TotalCount)> GetAllDoctorsAsync(PaginationAndSearchDTO request)
        {
            var (doctors, totalCount) = await _adminRepository.GetAllDoctorsAsync(request);

            var doctorDtos = doctors.Select(d => new DoctorDTO
            {
                FullName = d.User.FullName,
                Email = d.User.Email,
                DateOfBirth = Convert.ToString(d.User.DateOfBirth),
                Gender = d.User.Gender.ToString(),
                PhoneNumber = d.User.PhoneNumber,
                ImageUrl = d.User.ImageUrl,
                Specilization = d.Specialization.SpecializationName
            });

            return (doctorDtos, totalCount);
        }

        public async Task<int> TotalNumberOfPatients()
        {
            return await _adminRepository.NumberOfPatients();


        }

        public async Task<(IEnumerable<CustomUser>, int)> GetAllPatientsAsync(PaginationAndSearchDTO request)
        {
            var (patients, totalcount) = await _adminRepository.GetAllPatientsAsync(request);


            var patientDTO = patients.Select(p => new PatientDTO
            {
                Email = p.Email,
                FullName = p.FullName,
                Gender = p.Gender.ToString(),
                DateOfBirth = p.DateOfBirth.ToShortDateString(),
                PhoneNumber = p.PhoneNumber,
                ImageUrl = p.ImageUrl.ToString()
            });

            return (patients, totalcount);
        }

        public async Task<PatientDTO> GetPatientByIdAsync(string patientId)
        {
            var patient = await _adminRepository.GetPatientById(patientId);
            if (patient == null)
            {
                return null;
            }

            return new PatientDTO
            {
                FullName = patient.FullName,
                Email = patient.Email,
                PhoneNumber = patient.PhoneNumber,
                Gender = patient.Gender.ToString(),
                DateOfBirth = patient.DateOfBirth.ToShortDateString(),
                ImageUrl = patient.ImageUrl
            };
        }

        public async Task<IEnumerable<TopFiveSpecalizationDTO>> GetTopSpecializationsAsync()
        {
            var completeBookings = await _adminRepository.GetTopFiveSpecalizationsAsync();

            var topFiveSpecalizations = completeBookings
                .GroupBy(b => b.Appointement.Doctor.Specialization.SpecializationName)
                .Select(g => new TopFiveSpecalizationDTO
                {
                    SpecalizationName = g.Key,
                    RequestCount = g.Count()
                })
                .OrderByDescending(b => b.RequestCount)
                .Take(5)
                .ToList();

            return topFiveSpecalizations;
        }

        public async Task<IEnumerable<TopTenDoctorDTO>> GetTopTenDoctors()
        {
            var completedRequests = await _adminRepository.GetTopTenDoctors();

            var TopTenDoctors = completedRequests
                .GroupBy(b => new { b.Appointement.Doctor.User.FullName, b.Appointement.Doctor.Specialization.SpecializationName, b.Appointement.Doctor.User.ImageUrl })
                .Select(g => new TopTenDoctorDTO
                {
                    FullName = g.Key.FullName,
                    Specilization = g.Key.SpecializationName,
                    Image = g.Key.ImageUrl,
                    RequestCount = g.Count()
                })
                .OrderByDescending(b => b.RequestCount)
                .Take(10)
                .ToList();

            return TopTenDoctors;
        }

        public async Task<RequestsDTO>GetNumberOfRequestsAsync()
        {
            return await _adminRepository.GetNumberOfRequests();
        }
    }
}


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
using Microsoft.AspNetCore.Hosting;
using Application.Contracts;
namespace Application.Services
{
    public class AdminServices : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly UserManager<CustomUser> _userManager;
        private readonly ISpecializationRepository _specilizationRepository;
        private readonly IEmailSender _emailSender;



        public AdminServices(IAdminRepository adminRepository, UserManager<CustomUser> userManager, ISpecializationRepository specilizationRepository, IEmailSender emailSender)
        {
            _adminRepository = adminRepository;
            _userManager = userManager;
            _specilizationRepository = specilizationRepository;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Registers a new doctor asynchronously. The registration process includes uploading an image, creating a new user account, 
        /// adding the user to a specific role, and creating a doctor entity with a specialization. 
        /// </summary>
        /// <param name="model">The data transfer object containing all the necessary information for registering a doctor, 
        /// including personal details, email, password, and an image file.</param>
        /// <returns>
        /// A boolean value indicating the success of the doctor registration process. 
        /// Returns true if the registration is successful, otherwise false.
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if no image is uploaded, if user creation fails with a list of errors, 
        /// or if other exceptions occur during the process.
        /// </exception>
        public async Task<bool> AddDocotorAsync(DoctorRegisterDTO model)
        {

            if (model.ImageUrl == null || model.ImageUrl.Length == 0)
            {
                throw new Exception("No Image were Uploaded");
            }

            // Generate a unique file name to prevent overwriting existing files
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageUrl.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // Save the image to the 'wwwroot/images' folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageUrl.CopyToAsync(stream);
            }

            var imageUrl = $"images/{fileName}";


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
                ImageUrl = imageUrl,
                AccountRole = AccountRole.Doctor

            };
            var userResult = await _userManager.CreateAsync(user, model.Password);
            if (!userResult.Succeeded)
            {
                var errors = userResult.Errors.Select(e => e.Description);
                throw new Exception($"User creation failed: {string.Join(", ", errors)}");
            }
            await _userManager.AddToRoleAsync(user, AccountRole.Doctor.ToString());


            //Bonus Email Sender
            SendEmailToDoctorAsync(user.Id);



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
                SpecializationId = specialization.SpecializationId,
                CreatedDate = DateTime.UtcNow
            };

            await _adminRepository.CreateNewDoctorAsync(doctor);
            return true;
        }

        /// <summary>
        /// Asynchronously retrieves the details of a specific doctor by their ID. The information is formatted into a DoctorDTO object.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose details are to be retrieved.</param>
        /// <returns>
        /// A <see cref="DoctorDTO"/> object containing the details of the specified doctor, such as email, full name, phone number, 
        /// specialization, gender, date of birth, and image URL. Returns null if no doctor with the specified ID is found.
        /// </returns>
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


        /// <summary>
        /// Deletes a doctor and their associated appointments based on the given doctor ID. 
        /// </summary>
        /// <param name="doctorId">The ID of the doctor to be deleted.</param>
        /// <returns>Returns true if the doctor is successfully deleted.</returns>
        /// <exception cref="System.Exception">Thrown if the doctor has existing bookings and therefore cannot be deleted.</exception>
        /// <remarks>
        /// The method first checks for any existing bookings for the specified doctor. 
        /// If there are existing bookings, an exception is thrown indicating that the doctor cannot be deleted.
        /// If there are no bookings, the doctor's appointments are deleted followed by the deletion of the doctor record.
        /// Additionally, the associated user record for the doctor is also deleted from the user management system.
        /// </remarks>
        public async Task<bool> DeleteDoctorAsync(int doctorId)
        {
            var doctor = await _adminRepository.GetDoctorByIdAsync(doctorId);
            if(doctor == null)
            {
                throw new Exception($"No Doctor Id: {doctorId} was found ");
            }
            var doctorbookings= await _adminRepository.GetBookingByDoctorId(doctorId);
            var doctorAppointment = await _adminRepository.GetAppointmentByDoctorId(doctorId);
            if(doctorbookings)
            {
                throw new Exception("This Doctor Can't Be Deleted , Because he as Appointments");
            }

            else
            {
                await _adminRepository.DeleteDoctorAppointmentAsync(doctorAppointment);
                await _adminRepository.DeleteDoctorAsync(doctor);
                var user = await _userManager.FindByIdAsync(doctor.UserId);
                _userManager.DeleteAsync(user);

            }
            return true;
        }

        /// <summary>
        /// Retrieves the total number of doctors currently registered in the system.
        /// </summary>
        /// <returns>
        /// An integer representing the total number of doctors.
        /// </returns>
        public int GetTotalNumOfDoctors()
        {
            return _adminRepository.NumOfDoctors();
        }

        /// <summary>
        /// Asynchronously updates the details of an existing doctor based on the provided doctor ID and update information. 
        /// The update process modifies both the user's identity information and the doctor's specialization.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor to be updated.</param>
        /// <param name="model">The data transfer object containing the updated information for the doctor, 
        /// such as email, date of birth, image URL, phone number, gender, name, and specialization.</param>
        /// <returns>
        /// A boolean value indicating the success of the update process. Returns true if the update is successful, otherwise false.
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if no doctor with the specified ID is found in the database.
        /// </exception>
        public async Task<bool> DoctorUpdateAsync( DoctorUpdateDTO model)
        {
            var doctor = await _adminRepository.GetDoctorByIdAsync(model.doctorId);
            if (doctor == null)
            {
                throw new Exception($"No Doctor with This ID: {model.doctorId} was found, Check DoctorDB again");
            }

            if (model.ImageUrl == null || model.ImageUrl.Length == 0)
            {
                throw new Exception("No Image were Uploaded");
            }

            // Generate a unique file name to prevent overwriting existing files
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageUrl.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // Save the image to the 'wwwroot/images' folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ImageUrl.CopyToAsync(stream);
            }

            var imageUrl = $"images/{fileName}";



            //Updating Identity Table
            var user = await _userManager.FindByIdAsync(doctor.UserId);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.DateOfBirth = Convert.ToDateTime(model.DateOfBirth);
                user.ImageUrl = imageUrl;
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

        /// <summary>
        /// Asynchronously retrieves a paginated list of doctors and their total count. This function allows for optional 
        /// search criteria to filter the doctors based on the provided parameters in the request.
        /// </summary>
        /// <param name="request">The data transfer object containing pagination settings and optional search criteria.</param>
        /// <returns>
        /// A tuple where the first element is an IEnumerable of <see cref="DoctorDTO"/> representing the list of doctors, 
        /// and the second element is an integer representing the total count of doctors. Each <see cref="DoctorDTO"/> includes 
        /// details like full name, email, date of birth, gender, phone number, image URL, and specialization of the doctor.
        /// </returns>
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

        /// <summary>
        /// Asynchronously retrieves the total number of patients registered in the system.
        /// </summary>
        /// <returns>
        /// An integer representing the total number of patients.
        /// </returns>
        public async Task<int> TotalNumberOfPatients()
        {
            return await _adminRepository.NumberOfPatients();
        }

        /// <summary>
        /// Asynchronously retrieves a paginated list of patients along with the total count. This function supports pagination 
        /// and optional search criteria based on the provided parameters in the request.
        /// </summary>
        /// <param name="request">The data transfer object containing pagination settings and optional search criteria.</param>
        /// <returns>
        /// A tuple where the first element is an IEnumerable of <see cref="CustomUser"/> representing the list of patients, 
        /// and the second element is an integer representing the total count of patients. The patient data is transformed into 
        /// <see cref="PatientDTO"/> objects, including details such as email, full name, gender, date of birth, phone number, 
        /// and image URL.
        /// </returns>
        public async Task<(IEnumerable<PatientDTO>, int)> GetAllPatientsAsync(PaginationAndSearchDTO request)
        {
            var (patients, totalcount) = await _adminRepository.GetAllPatientsAsync(request);


            var patientDTO = patients.Select(p => new PatientDTO
            {
                Email = p.Email,
                FullName = p.FullName,
                Gender = p.Gender.ToString(),
                DateOfBirth = p.DateOfBirth.ToShortDateString(),
                PhoneNumber = p.PhoneNumber,
                ImageUrl = p.ImageUrl?.ToString()
            });

            return (patientDTO, totalcount);
        }


        /// <summary>
        /// Asynchronously retrieves the details of a specific patient by their unique identifier. 
        /// The patient's information is formatted into a PatientDTO object.
        /// </summary>
        /// <param name="patientId">The unique identifier of the patient whose details are to be retrieved.</param>
        /// <returns>
        /// A <see cref="PatientDTO"/> object containing the details of the specified patient, such as full name, email, 
        /// phone number, gender, date of birth, and image URL. Returns null if no patient with the specified ID is found.
        /// </returns>
        public async Task<PatientDTO> GetPatientByIdAsync(string patientId)
        {
            var patient = await _adminRepository.GetPatientById(patientId);
            if (patient == null || patient.AccountRole != AccountRole.Patient)
            {
                throw new Exception($"No Patients with This ID was Found");
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

        /// <summary>
        /// Asynchronously retrieves the top five specializations based on the number of completed bookings. 
        /// This function groups the bookings by specialization and calculates the total count for each, 
        /// returning the five specializations with the highest counts.
        /// </summary>
        /// <returns>
        /// An IEnumerable of <see cref="TopFiveSpecalizationDTO"/> objects, each representing a specialization. 
        /// Each object includes the specialization name and the count of requests or bookings associated with it, 
        /// sorted in descending order by the request count.
        /// </returns>
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

        /// <summary>
        /// Asynchronously retrieves the top ten doctors based on the number of completed requests or bookings. 
        /// This function groups the bookings by doctor and calculates the total count for each, 
        /// returning the ten doctors with the highest counts.
        /// </summary>
        /// <returns>
        /// An IEnumerable of <see cref="TopTenDoctorDTO"/> objects, each representing a doctor. 
        /// Each object includes the doctor's full name, specialization, image URL, and the count of requests or bookings associated with them, 
        /// sorted in descending order by the request count.
        /// </returns>
        public async Task<IEnumerable<TopTenDoctorDTO>> GetTopTenDoctorsAsync()
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

        /// <summary>
        /// Asynchronously retrieves the total number of requests or bookings made in the system. 
        /// The information is encapsulated in a RequestsDTO object.
        /// </summary>
        /// <returns>
        /// A <see cref="RequestsDTO"/> object containing the total number of requests. 
        /// This may include various statistics or counts related to the requests.
        /// </returns>
        public async Task<RequestsDTO> GetNumberOfRequestsAsync()
        {
            return await _adminRepository.GetNumberOfRequests();
        }

        /// <summary>
        /// Asynchronously retrieves the number of doctors who have been added to the system in the last 24 hours.
        /// </summary>
        /// <returns>
        /// An integer representing the count of doctors added in the last 24 hours.
        /// </returns>
        public async Task<int> GetNumberOfDoctorsAddedLast24HoursAsync()
        {
            return await _adminRepository.GetNumberOfDoctorsAddedLast24HoursAsync();
        }

        /// <summary>
        /// Asynchronously sends an email to a specific doctor identified by their ID. 
        /// The email includes a temporary password and instructions for the doctor to change their password upon first login.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor to whom the email is to be sent.</param>
        /// <remarks>
        /// This function generates a password reset token, resets the doctor's password to a temporary one, and sends an email 
        /// with the temporary password and instructions for changing it.
        /// </remarks>
        public async Task SendEmailToDoctorAsync(string doctorId)
        {
            var doctor = await _userManager.FindByIdAsync(doctorId);
            if (doctor != null)
            {
                // Prepare the email content
                var emailSubject = "Your Doctor Account Details";
                var emailMessage = $"Your account has been created with the following details:\n\n" +
                                   $"Email: {doctor.Email}\n" +
                                   $"Password: Test@2023\n\n" +
                                   "Please change your password upon first login.";

                // Send email
                await _emailSender.SendEmailAsync(doctor.Email, emailSubject, emailMessage);
            }
            else
            {
                // Handle the case where the doctor is not found
                throw new Exception("Doctor not found.");
            }
        }

    }
}

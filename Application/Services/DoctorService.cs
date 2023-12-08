
using Application.Contracts;
using Domain.DTOS;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Application.Services
{
    public class DoctorService : IDoctorService
    {

        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IDoctorRepository _doctorRepository;
        private readonly IAdminService _adminService;
        private readonly IPatientRepository _patientRepository;
       
        public DoctorService
            (UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, IDoctorRepository doctorRepository, IAdminService adminService, IPatientRepository patientRepository,IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _doctorRepository = doctorRepository;
            _adminService = adminService; 
            _patientRepository = patientRepository;
            
        }

        /// <summary>
        /// Asynchronously attempts to log in a doctor using the provided email and password credentials.
        /// </summary>
        /// <param name="model">The data transfer object containing the login credentials, which includes the doctor's email 
        /// and password.</param>
        /// <returns>
        /// A boolean value indicating the success of the login attempt. Returns true if the login is successful, otherwise false.
        /// </returns>
        public async Task<bool> DoctorLoginAsync(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                return result.Succeeded;
            }
            return false;
        }

        /// <summary>
        /// Asynchronously retrieves all appointments scheduled for a specific doctor. 
        /// The appointments are formatted into a collection of DoctorBookingsDTO objects.
        /// </summary>
        /// <param name="doctorId">The unique identifier of the doctor whose appointments are to be retrieved.</param>
        /// <returns>
        /// An IEnumerable of <see cref="DoctorBookingsDTO"/> objects, each representing an appointment. 
        /// Each object includes details such as the patient's name, age, phone number, appointment day, 
        /// start and end times, and the patient's image URL.
        /// </returns>
        public async Task<IEnumerable<DoctorBookingsDTO>> GetAppointmentsForDoctorAsync(int doctorId)
        {


            var bookings = await _doctorRepository.GetDoctorApptAsync();
            var doctorSchedules = bookings
                .Where(b => b.Appointement.DoctorId == doctorId)
                .Select(group => new DoctorBookingsDTO
                {
                    PatientName = group.Patient.FullName,
                    Age = group.Patient.DateOfBirth.ToShortTimeString(),
                    PhoneNumber = group.Patient.PhoneNumber,
                    Day = group.Appointement.DaysOfTheWeek.ToString(),
                    StartTime = group.Time.StartTime.ToShortTimeString(),
                    EndTime = group.Time.EndTime.ToShortTimeString(),
                    Image = group.Patient.ImageUrl
                });
            return doctorSchedules;
        }


        /// <summary>
        /// Asynchronously adds a new doctor's appointment, including updating the doctor's price and creating appointment time slots.
        /// </summary>
        /// <param name="doctorDTO">A <see cref="AddAppointmentDTO"/> object containing the details of the new appointment, 
        /// including the doctor's ID, days of the week, price, start time, and end time.</param>
        /// <returns>
        /// A boolean value indicating whether the addition of the doctor's appointment was successful.
        /// </returns>
        public async Task<bool>AddDoctorAppointment(AddAppointmentDTO doctorDTO)
        {
            //Updating Doctor Table
            await _doctorRepository.UpdateDoctorPrice(doctorDTO.DoctorId, doctorDTO.Price);

           
            //Updating Appointment Table
            var newAppointment = new Appointement
            { 
                DaysOfTheWeek = doctorDTO.DaysOfTheWeek , 
                DoctorId = doctorDTO.DoctorId 
            };
            await _doctorRepository.AddDoctorAppointmentAsync(newAppointment);

       
            var newTime = new Time 
            {
                AppointmentId = newAppointment.AppointmentId, 
                StartTime = doctorDTO.StartTime, 
                EndTime = doctorDTO.EndTime 
            };
            await _doctorRepository.UpdateTimeAppointment(newTime);

            return true;
        }

        /// <summary>
        /// Asynchronously deletes a time appointment slot if it is not already booked and canceled.
        /// </summary>
        /// <param name="timeId">The unique ID of the time appointment slot to be deleted.</param>
        /// <returns>
        /// A boolean value indicating whether the deletion of the time appointment slot was successful.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the specified time appointment slot is already booked and not canceled, or if the appointment slot with the provided ID is not found.
        /// </exception>
        public async Task<bool> DeleteTimeAppointmentAsync(int timeId)
        {
            var timeAppointment = await _doctorRepository.FindAppointmentTimeById(timeId); 
            if(timeAppointment == null)
            {
                throw new Exception($"The Appointment ID : {timeId} is not found");
            }

            var bookingAppointment= await _doctorRepository.FindBookingByTimeId(timeId);

            if(timeAppointment.TimeId == timeId && bookingAppointment.Status == BookingStatus.Canceled) 
            {
                await _doctorRepository.DeleteAppointmentTime(timeId); 
            }
            else
            {
                throw new Exception($"The Appointment ID : {timeId} is already Booked and you can't delete it ");
            }
            return true;
        }

        /// <summary>
        /// Asynchronously updates the appointment time slot for a doctor if it is not already booked by a patient.
        /// </summary>
        /// <param name="timeId">The unique ID of the appointment time slot to be updated.</param>
        /// <param name="model">The appointment update information containing the new start and end times.</param>
        /// <returns>
        /// A boolean value indicating whether the update of the appointment time slot was successful.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the specified appointment time slot is already booked by a patient or if the appointment slot with the provided ID is not found.
        /// </exception>
        public async Task<bool> DoctorUpdateAppointmentAsync(int timeId , UpdateAppointmentDTO model)
        {
            var timeAppointment = await _doctorRepository.FindAppointmentTimeById(timeId);
            if (timeAppointment == null)
            {
                throw new Exception($"No appointment with ID : {timeId} was found");

            }
            var BookingAppointment = await _patientRepository.FindyBookingByTimeId(timeId);
            if(BookingAppointment)
            {
                throw new Exception("The Time you are Trying to update is already booked");
            }
            else
            {
                timeAppointment.StartTime = model.StartTime;
                timeAppointment.EndTime = model.EndTime;
            }
    
            await _doctorRepository.DoctorAppointmentUpdateAsync(timeAppointment);
            return true; 
        }

        /// <summary>
        /// Asynchronously confirms a check-up appointment by updating its status in the system.
        /// </summary>
        /// <param name="bookingId">The unique ID of the check-up appointment to be confirmed.</param>
        /// <returns>
        /// A boolean value indicating whether the confirmation of the check-up appointment was successful.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when the specified check-up appointment with the provided ID is not found.
        /// </exception>
        public async Task<bool>DoctorConfirmCheckUp(int bookingId )
        {
          var confirmCheckupTime =  await _doctorRepository.ConfirmCheckup(bookingId);
            if(!confirmCheckupTime)
            {
                throw new Exception($"No appointment with ID : {bookingId} was found");
            }
            return true;
        }
    }
}


using Application.Contracts;
using Domain.DTOS;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        public async Task<(IEnumerable<DoctorBookingsDTO>, int totalCounts)> GetAppointmentsForDoctorAsync(int doctorId , PaginationAndSearchDTO request)
        {
            var (doctorBookings, totalCounts) = await _doctorRepository.GetDoctorApptAsync(request , doctorId);

            if(doctorBookings == null || !doctorBookings.Any())
            {
                throw new Exception("No Appointments found for this Doctor");
            }

            var selectedBookings = doctorBookings.Select(group => new DoctorBookingsDTO
            {
                PatientName = group.Booking?.Patient.FullName ?? "Unknown",
                Age = group.Booking?.Patient?.DateOfBirth.ToString() ?? "Unknown",
                Day = group.Days.ToString(),
                PhoneNumber = group.Booking?.Patient?.PhoneNumber ?? "Unknown",
                StartTime = group.Times.FirstOrDefault()?.StartTime ?? "Unknown",
                EndTime = group.Times.FirstOrDefault()?.EndTime ?? "Unknown",
                Image = group.Booking?.Patient?.ImageUrl ?? "null" // Use a default or placeholder image if null
            }).ToList();


            return (selectedBookings, totalCounts);
        }


        /// <summary>
        /// Adds new appointments for a doctor based on the provided details. This includes updating the doctor's price and scheduling multiple appointments on different days and times.
        /// </summary>
        /// <param name="doctorDTO">An object containing details for the doctor's appointments, including doctor ID, new price, and a list of day appointments.</param>
        /// <returns>Returns true if the appointments are successfully added.</returns>
        /// <remarks>
        /// This method first updates the doctor's price using the provided doctor ID and price. 
        /// It then iterates over each day's appointments specified in the doctorDTO. 
        /// For each day, it creates a new Appointment object and adds multiple time slots for that appointment. 
        /// Each time slot, defined by a start and end time, is added to the doctor's schedule.
        /// </remarks>
        public async Task<bool>AddDoctorAppointmentAsync(AddAppointmentDTO doctorDTO)
        {

            await _doctorRepository.UpdateDoctorPrice(doctorDTO.DoctorId, doctorDTO.Price);

            foreach (var dayAppointment in doctorDTO.DayAppointments)
            {
                // Create a new appointment
                var appointment = new Appointement
                {
                    DoctorId = doctorDTO.DoctorId,
                    Days = dayAppointment.Day, // Assuming Day is a property like "Monday", "Tuesday", etc.
                    Times = new List<Time>()
                };

                await _doctorRepository.AddDoctorAppointment(appointment);

                // Process each time range for the day
                foreach (var item in dayAppointment.TimeDTOs)
                {

                    var startTime = item.StartTime;
                    var endTime = item.EndTime;

                        // Create a new Time object and add it to the appointment's Times list
                        var time = new Time
                        {
                            StartTime = startTime,
                            EndTime = endTime,
                            AppointmentId = appointment.AppointmentId
                        };
                       
                        await _doctorRepository.UpdateTimeAppointment(time);
                }
            }
            return true;
        }

        /// <summary>
        /// Deletes a time appointment based on the given appointment ID. 
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment to be deleted.</param>
        /// <returns>Returns true if the appointment is successfully deleted.</returns>
        /// <exception cref="System.Exception">Thrown if the appointment with the specified ID is not found, 
        /// or if the appointment is already booked and cannot be canceled.</exception>
        /// <remarks>
        /// The method first checks if the appointment exists. If it does not, an exception is thrown.
        /// If the appointment exists but is already booked and not canceled, an exception is thrown indicating it cannot be deleted.
        /// If the appointment is either not booked or is booked but canceled, it is then deleted.
        /// </remarks
        public async Task<bool> DeleteTimeAppointmentAsync(int appointmentId)
        {
            var timeAppointment = await _doctorRepository.FindAppointmentByAppointmentId(appointmentId); 
            if(timeAppointment == null)
            {
                throw new Exception($"The Appointment ID : {appointmentId} is not found");
            }

            var bookingAppointment= await _doctorRepository.FindBookingByAppointmentId(appointmentId);
            if (bookingAppointment == null || bookingAppointment.Status == BookingStatus.Canceled)
            {
                await _doctorRepository.DeleteAppointmentTime(timeAppointment);
                return true;
            }
            else
            {
                throw new Exception($"The Appointment ID : {appointmentId} is already booked and cannot be deleted.");
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
        public async Task<bool> DoctorUpdateAppointmentAsync(int appointmentId , UpdateAppointmentDTO model)
        {
            var timeAppointment = await _doctorRepository.FindAppointmentByAppointmentId(appointmentId);
            if (timeAppointment == null)
            {
                throw new Exception($"No appointment with ID : {appointmentId} was found");

            }
            var BookingAppointment = await _patientRepository.FindyBookingByAppoitmentId(appointmentId);
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



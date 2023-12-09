
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
    public class PatientService  : IPatientService
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICouponRepository _couponRepository;

        public PatientService(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager , IPatientRepository patientRepository, IDoctorRepository doctorRepository, ICouponRepository couponRepository , IAdminRepository adminRepository )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;
            _couponRepository = couponRepository;
        }

        /// <summary>
        ///  Registers a new patient asynchronously. This process involves creating a new user record
        /// with the provided patient details, saving an optional profile image, and assigning the patient role.
        /// </summary>
        /// <param name="model">
        /// The patient registration data transfer object containing all necessary patient information,
        /// including name, email, password, contact details, date of birth, gender, and an optional profile image
        /// </param>
        /// <returns>
        /// A boolean value indicating the success of the registration process. 
        /// Returns true if the patient is successfully registered, otherwise false.
        /// </returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> RegisterPatientAsync(PatientRegisterDTO model)
        {
            string fileName = null; 

            // Check if an image is provided
            if (model.ImageUrl != null && model.ImageUrl.Length > 0)
            {
                // Generate a unique file name to prevent overwriting existing files
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageUrl.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                // Save the image to the 'wwwroot/images' folder
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageUrl.CopyToAsync(stream);
                }
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
                ImageUrl= imageUrl,
                AccountRole = AccountRole.Patient
            };
            var userResult = await _userManager.CreateAsync(user, model.Password);
            if (!userResult.Succeeded)
            {
                throw new Exception(userResult.Errors.ToString());
            }
            await _userManager.AddToRoleAsync(user, AccountRole.Patient.ToString());
            return true;
        }

        /// <summary>
        /// Attempts to log in a patient asynchronously using their email and password. This function checks if the user exists 
        /// and then tries to sign in using the provided credentials.
        /// </summary>
        /// <param name="model">The data transfer object containing the login credentials, which includes the patient's email 
        /// and password.</param>
        /// <returns>
        /// A boolean value indicating the success of the login attempt. Returns true if the login is successful, otherwise false.
        /// </returns>
        public async Task<bool> LoginPatientAsync(LoginDTO model)
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
        /// Asynchronously retrieves a collection of appointments grouped by doctor. Each doctor's appointments are further 
        /// grouped by day of the week, providing a detailed schedule including time slots.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="AppointmentDTO"/> objects. Each object contains the doctor's name, specialization, 
        /// consultation price, and a list of <see cref="DayScheduleDTO"/> objects. Each <see cref="DayScheduleDTO"/> object 
        /// represents a specific day of the week and includes a list of time slots (start time to end time) for available appointments.
        /// </returns>
        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsForDoctorAsync()
        {
            var Times = await _patientRepository.GetDoctorApptAsync();
            var doctorSchedules = Times
                .GroupBy(a => new { a.Appointement.Doctor.User.FullName, a.Appointement.Doctor.Specialization.SpecializationName, a.Appointement.Doctor.Price, a.Appointement.Days })
                .Select(doctorGroup => new AppointmentDTO
                {
                    DoctorName = doctorGroup.Key.FullName,
                    Specailization = doctorGroup.Key.SpecializationName,
                    Price = doctorGroup.Key.Price,
                    AvailableDay = doctorGroup
                        .GroupBy(a => a.Appointement.Days) // Group by Day
                        .Select(dayGroup => new DayScheduleDTO
                        {
                            Day = dayGroup.Key.ToString(),
                            TimeSlots = dayGroup
                          .Select(a => $"{a.StartTime} TO {a.EndTime}")
                           .ToList()
                        })
               .ToList()
                }).ToList();
            return doctorSchedules;
        }


        /// <summary>
        /// Creates a new booking for a specified appointment time and patient. Optionally applies a coupon if provided and valid.
        /// </summary>
        /// <param name="timeId">The ID of the appointment time for which the booking is to be made.</param>
        /// <param name="patientId">The ID of the patient making the booking.</param>
        /// <param name="couponName">Optional. The name of the coupon to be applied to the booking. If null or not provided, no coupon is applied.</param>
        /// <returns>
        /// A boolean value indicating whether the booking was successfully created.
        /// Returns true if the booking is successfully created, otherwise false.
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if the appointment time specified by timeId is not found, if the appointment time is already booked,
        /// if the coupon is invalid or inactive, or if the patient does not meet the criteria to use the coupon.
        /// </exception>
        public async Task<bool>CreateNewBooking(int appointmentId, string patientId , string? couponName = null)
        {
            var appointmentTime = await _doctorRepository.FindAppointmentByAppointmentId(appointmentId);
            if (appointmentTime == null)
            {
                throw new Exception($"Appointment time with ID {appointmentId} not found.");
            }
            var originalPrice = appointmentTime.Appointement.Doctor.Price;
            var priceAfterCoupon = appointmentTime.Appointement.Doctor.Price;


            var booking = await _patientRepository.FindyBookingById(appointmentId);

            if(booking != null)
            {
                throw new Exception($"Appointment time with ID {appointmentId} is already Booked !.");
            }

            //Checking On Coupon
             bool isCouponUsed = false;
            if (!string.IsNullOrWhiteSpace(couponName))
            {
                var coupon = await _couponRepository.FindCouponByName(couponName);
                if (coupon != null && coupon.IsActive)
                {
                    // Check if the patient has completed enough requests
                    int completedRequests = await _couponRepository.GetNumberOfCompletedRequestByPatientId(patientId);
                    if (completedRequests == 5)
                    {
                        coupon.PatientId = patientId;
                        if(coupon.PatientId == patientId && completedRequests == 5)
                        {
                            throw new Exception("You have already Used This Coupon!");
                        }
                        isCouponUsed = true;
                        priceAfterCoupon = (int)appointmentTime.Appointement.Doctor.Price - 50;
                    }
                    else if(completedRequests == 10)
                    {
                        coupon.PatientId = patientId;
                        if (coupon.PatientId == patientId && completedRequests == 11)
                        {
                            throw new Exception("You have already Used This Coupon!");
                        }
                        isCouponUsed = true;
                        priceAfterCoupon = (int)appointmentTime.Appointement.Doctor.Price - 100;
                    }
                    
                    else
                    {
                        throw new Exception("Patient does not have enough completed requests to use the coupon.");
                    }
                }
                else
                {
                    throw new Exception("Invalid or inactive coupon.");
                }
            }

                var newBooking = new Booking
                {
                    AppointmentId = appointmentTime.AppointmentId,
                    PatientId = patientId,
                    IsCouponUsed = isCouponUsed,
                    Price = (int)originalPrice,
                    PriceAfterCoupon = (int)priceAfterCoupon, 
                };
                await _patientRepository.CreateBookingAsync(newBooking);
          
            
            return true;
    
        }

        /// <summary>
        /// Asynchronously cancels a booking based on the provided booking ID. It checks if the booking exists and then proceeds to cancel it.
        /// </summary>
        /// <param name="bookingId">The ID of the booking to be canceled.</param>
        /// <returns>
        /// A boolean value indicating the success of the cancellation process. 
        /// Returns true if the cancellation is successful.
        /// </returns>
        /// <exception cref="Exception">
        /// Throws an exception if no appointment with the given booking ID is found.
        /// </exception>
        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var canceledAppointment =  await _patientRepository.CancelAppointment(bookingId);
            if(!canceledAppointment)
            {
                throw new Exception($"No Appointment with ID: {bookingId} is found");
            }
            return true;
        }

        /// <summary>
        /// Asynchronously retrieves all bookings specifically associated with a given patient. This function filters bookings 
        /// by the patient's ID and formats the data into a collection of PatientBookingDTO objects.
        /// </summary>
        /// <param name="patientId">The unique identifier for the patient whose bookings are to be retrieved.</param>
        /// <returns>
        /// An IEnumerable collection of <see cref="PatientBookingDTO"/> objects, each representing a booking for the specified patient. 
        /// Each object includes details such as the doctor's name, specialization, price, phone number, appointment day, 
        /// appointment image, start and end times of the appointment, and the booking status.
        /// </returns>
        public async Task<IEnumerable<PatientBookingDTO>>GetPatientSpecificBookingsAsync(string patientId)
        {
            var bookings = await _patientRepository.GetPatientBookings();
            if(bookings == null)
            {
                throw new Exception("Error");
            }
            var patientSchedule = bookings
                .Where(p => p.Appointement.Booking.PatientId == patientId)
                .Select(group => new PatientBookingDTO
                {
                    DoctorName = group.Appointement.Doctor.User.FullName,
                    Specailization = group.Appointement.Doctor.Specialization.SpecializationName,
                    Price = group.Appointement.Doctor.Price.ToString(),
                    PhoneNumber = group.Appointement.Doctor.User.PhoneNumber,
                    Day = group.Appointement.Days.ToString(),
                    FinalPrice = group.Appointement.Booking.PriceAfterCoupon.ToString(),
                    Image = group.Appointement.Doctor.User.ImageUrl,
                    StartTime = group.Appointement.Times.FirstOrDefault()?.StartTime,
                    EndTime = group.Appointement.Times.FirstOrDefault()?.EndTime,
                    BookingStatus = group.Appointement.Booking.Status.ToString()
                });
            return patientSchedule;
        }
    }
}

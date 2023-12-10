
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
            CultureInfo provider = new CultureInfo("en-US");
            string[] formats = { "yyyy/MM/dd", "yyyy/M/dd", "yyyy/M/d", "yyyy/MM/d", "dd/MM/yyyy", "dd/M/yyyy", "d/MM/yyyy", "d/M/yyyy" };
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
                DateOfBirth = DateTime.ParseExact(model.DateOfBirth , formats , provider),
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
        /// Retrieves a paginated and grouped list of doctor appointments based on the specified search and pagination criteria.
        /// </summary>
        /// <param name="request">An object containing the search criteria and pagination parameters.</param>
        /// <remarks>
        /// This method fetches appointments from the repository using the provided <paramref name="request"/>,
        /// groups them by doctor's full name, specialization, price, and day, and then maps them to <see cref="AppointmentDTO"/>.
        /// If no appointments are found, it throws an exception.
        /// </remarks>
        /// <returns>
        /// A tuple containing a list of <see cref="AppointmentDTO"/> representing the grouped appointments and
        /// the total count of appointments found for the given criteria.
        /// </returns>
        /// <exception cref="Exception">Thrown when no appointments are found.</exception>
        public async Task<(IEnumerable<AppointmentDTO>, int totalCount)> GetAppointmentsForDoctorAsync(PaginationAndSearchDTO request)
        {
            var (times, totalCount)  = await _patientRepository.GetDoctorApptAsync(request);
            if (times == null)
            {
                throw new Exception("No Appointments were found");
            }

            var doctorSchedules = times
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
            return (doctorSchedules , totalCount);
        }


        /// <summary>
        /// Creates a new booking for a given appointment. It optionally applies a coupon if provided and valid. 
        /// It checks if the appointment time is available and if the coupon can be applied based on the number of completed requests by the patient.
        /// </summary>
        /// <param name="appointmentId">The ID of the appointment for which the booking is being created.</param>
        /// <param name="patientId">The ID of the patient who is making the booking.</param>
        /// <param name="couponName">The name of the coupon to apply to the booking. This is optional.</param>
        /// <returns>Returns true if the booking is successfully created.</returns>
        /// <exception cref="System.Exception">Thrown if the appointment time is not found, 
        /// if the appointment time is already booked, if the coupon is invalid, inactive, 
        /// or already used, or if the patient does not have enough completed requests to use the coupon.</exception>
        public async Task<bool> CreateNewBookingAsync(int appointmentId, string patientId , string? couponName = null)
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
            return await _patientRepository.CancelAppointment(bookingId);
                   
        }

        /// <summary>
        /// Retrieves a list of bookings specific to a patient. Each booking includes details about the doctor, 
        /// the specialization, appointment price, contact information, and appointment times.
        /// </summary>
        /// <param name="patientId">The ID of the patient for whom to retrieve bookings.</param>
        /// <returns>A collection of <see cref="PatientBookingDTO"/> objects, each representing a booking made by the specified patient.</returns>
        /// <exception cref="System.Exception">Thrown if there is an error retrieving bookings from the repository.</exception>
        /// <remarks>
        /// This method fetches all patient bookings and filters them based on the provided patient ID.
        /// If the patient has no bookings or there is an issue retrieving bookings, an exception is thrown.
        /// Each booking includes detailed information such as the doctor's name, specialization, appointment times, 
        /// and final price after any applied coupons.
        /// </remarks>
        public async Task<IEnumerable<PatientBookingDTO>>GetPatientSpecificBookingsAsync(string patientId)
        {
            var bookings = await _patientRepository.GetPatientBookings();
            if(bookings == null )
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

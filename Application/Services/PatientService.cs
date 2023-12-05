
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
    public class PatientService 
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IPatientRepository _patientRepository;
        private readonly IDoctorRepository _doctorRepository;

        public PatientService(UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager , IPatientRepository patientRepository , IDoctorRepository doctorRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _patientRepository = patientRepository;
            _doctorRepository = doctorRepository;   
        }

        public async Task<bool> RegisterPatientAsync(PatientRegisterDTO model)
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
        public async Task<IEnumerable<AppointmentDTO>> GetAppointmentsForDoctorAsync()
        {
            var Times = await _patientRepository.GetDoctorApptAsync();
            var doctorSchedules = Times
                .GroupBy(a => new { a.Appointements.Doctor.User.FullName, a.Appointements.Doctor.Specialization.SpecializationName, a.Appointements.Doctor.Price, a.Appointements.DaysOfTheWeek })
                .Select(doctorGroup => new AppointmentDTO
                {
                    DoctorName = doctorGroup.Key.FullName,
                    Specailization = doctorGroup.Key.SpecializationName,
                    Price = doctorGroup.Key.Price,
                    AvailableDay = doctorGroup
                        .GroupBy(a => a.Appointements.DaysOfTheWeek) // Group by Day
                        .Select(dayGroup => new DayScheduleDTO
                        {
                            Day = dayGroup.Key.ToString(),
                            TimeSlots = dayGroup
                          .Select(a => $"{a.StartTime.ToString(@"hh\:mm tt")} TO {a.EndTime.ToString(@"hh\:mm tt")}")
                           .ToList()
                        })
               .ToList()
                }).ToList();
            return doctorSchedules;
        }

        public async Task<bool>CreateNewBooking(int timeId, string PatientId)
        {
            var test = await _doctorRepository.FindAppointmentTimeById(timeId);
            if (test == null)
            {
                throw new Exception($"Appointment time with ID {timeId} not found.");
            }

            var booking = await _patientRepository.FindyBookingById(timeId);

            if(booking == null)
            {
                var newBooking = new Booking
                {
                    TimeId = test.TimeId,
                    AppointmentId = test.AppointmentId,
                    PatientId = PatientId
                };
                await _patientRepository.CreateBookingAsync(newBooking);
            }
            else
            {
                throw new Exception($"Appointment time with ID {timeId} is already Booked !.");
            }
            return true;
    
        }

        public async Task<bool> CancelBookingAsync(int bookingId)
        {
            var canceledAppointment =  await _patientRepository.CancelAppointment(bookingId);
            if(canceledAppointment)
            {
                throw new Exception($"No Appointment with ID: {bookingId} is found");
            }
            return true;
        }

        public async Task<IEnumerable<PatientBookingDTO>>GetPatientSpecificBookingsAsync(string patientId)
        {
            var bookings = await _patientRepository.GetPatientBookings();
            var patientSchedule = bookings
                .Where(p => p.PatientId == patientId)
                .Select(group => new PatientBookingDTO
                {
                    DoctorName = group.Appointement.Doctor.User.FullName,
                    Specailization = group.Appointement.Doctor.Specialization.SpecializationName,
                    Price = group.Appointement.Doctor.Price.ToString(),
                    PhoneNumber = group.Appointement.Doctor.User.PhoneNumber,
                    Day = group.Appointement.DaysOfTheWeek.ToString(),
                    Image = group.Appointement.Doctor.User.ImageUrl,
                    StartTime = group.Time.StartTime.ToShortTimeString(),
                    EndTime = group.Time.EndTime.ToShortTimeString(),
                    BookingStatus = group.Status.ToString()
                });
            return patientSchedule;
        }
    }
}

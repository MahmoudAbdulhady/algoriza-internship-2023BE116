
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
    public class DoctorService 
    {

        private readonly UserManager<CustomUser> _userManager;
        private readonly SignInManager<CustomUser> _signInManager;
        private readonly IDoctorRepository _doctorRepository;
        private readonly AdminServices _adminService;
        private readonly IPatientRepository _patientRepository;
        public DoctorService
            (
            UserManager<CustomUser> userManager, 
            SignInManager<CustomUser> signInManager, 
            IDoctorRepository doctorRepository,
            AdminServices adminService,
            IPatientRepository patientRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _doctorRepository = doctorRepository;
            _adminService = adminService; 
            _patientRepository = patientRepository;
        }

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


            //    var groupedAppointments = await _context.Bookings
            //        .Where(b => b.DoctorId == doctorId)
            //        .GroupBy(b => b.PatientId)
            //.       Select(group => new PatientAppointmentGroupDTO
            //    {
            //    PatientId = group.Key,
            //    Appointments = group.Select(b => new DoctorAppointmentDTO
            //    {
            //        AppointmentId = b.AppointmentId,
            //        AppointmentTime = b.AppointmentTime,
            //        // Map other properties
            //      }).ToList()
            //     }).ToListAsync();














            //var Times = await _doctorRepository.GetDoctorApptAsync();
            //var doctorSchedules = Times
            //    .GroupBy(a=> new {a.Appointements.Doctor.User.FullName , a.Appointements.Doctor.Specialization.SpecializationName , a.Appointements.Doctor.Price , a.Appointements.DaysOfTheWeek})
            //    .Select(doctorGroup => new AppointmentDTO
            //    {
            //        DoctorName = doctorGroup.Key.FullName,
            //        Specailization= doctorGroup.Key.SpecializationName,
            //        Price = doctorGroup.Key.Price,
            //        AvailableDay = doctorGroup
            //            .GroupBy(a => a.Appointements.DaysOfTheWeek) // Group by Day
            //            .Select(dayGroup => new DayScheduleDTO
            //            {
            //              Day = dayGroup.Key.ToString(),
            //              TimeSlots = dayGroup
            //              .Select(a => $"{a.StartTime.ToString(@"hh\:mm tt")} TO {a.EndTime.ToString(@"hh\:mm tt")}")
            //               .ToList()
            //              })
            //   .ToList()
            //    }).ToList();
            //return doctorSchedules;
        }

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

            // TODO ask Ahmed about Time Format !!
            var newTime = new Time 
            {
                AppointmentId = newAppointment.AppointmentId, 
                StartTime = doctorDTO.StartTime, 
                EndTime = doctorDTO.EndTime 
            };
            await _doctorRepository.UpdateTimeAppointment(newTime);


            return true;

        
        }
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

            //var appointmentTime =  _doctorRepository.DeleteAppointmentTime(timeId);
            //if(appointmentTime == null)
            //{
            //    throw new Exception($"No appointment with ID : {timeId} aws found");
            //}
          
         
        }

        public async Task<bool> DoctorUpdateAppointmentAsync(int timeId , UpdateAppointmentDTO model)
        {
            var timeAppointment = await _doctorRepository.FindAppointmentTimeById(timeId);
            var BookingAppointment = await _patientRepository.FindyBookingByTimeId(timeId);
            if(timeAppointment == null)
            {
                throw new Exception($"No appointment with ID : {timeId} was found");

            }
            if(BookingAppointment)
            {
                throw new Exception("The Time you are Trying to update is already booked");
            }
            else
            {
                timeAppointment.StartTime = model.StartTime;
                timeAppointment.EndTime = model.EndTime;
            }
          

            //TODO Update the throw Exception 
            var updatedTime = await _doctorRepository.DoctorAppointmentUpdateAsync(timeAppointment);
            return true;
          
        }

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

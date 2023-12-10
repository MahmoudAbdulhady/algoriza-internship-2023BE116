
using Application.Contracts;
using Application.DTOS;
using Application.Services;
using Domain.DTOS;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Veezta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;
        private readonly ILogger<PatientController> _logger;
        public PatientController(IPatientService patientService, ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> PatientRegister([FromForm] PatientRegisterDTO model)
        {
            _logger.LogInformation("Action Started");
            if(ModelState.IsValid)
            {
                var patient = await _patientService.RegisterPatientAsync(model);
                if(patient)
                {
                    _logger.LogInformation("Action Ended");
                    return Ok("Patient Registered Successfully!");
                }
            }
            _logger.LogInformation("Action Ended");
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> PatientLogin([FromBody] LoginDTO model)
        {
            _logger.LogInformation("Action Started");
            if (ModelState.IsValid)
            {
                var patient = await _patientService.LoginPatientAsync(model);
                if (patient)
                {
                    _logger.LogInformation("Action Ended");
                    return Ok("Login Successfully!");
                }
            }
            _logger.LogInformation("Action Ended");
            return Unauthorized("Invalid Email or Password , Please Try Again !");
        }

        [HttpPost("GetDoctorAppointments")]
        public async Task<IActionResult> GetDoctorAppointments(PaginationAndSearchDTO request)
        {
            _logger.LogInformation("Action Started");
            var (appointments, totalCounts)= await _patientService.GetAppointmentsForDoctorAsync(request);
            _logger.LogInformation("Action Ended");
            return Ok(new { appointments, totalCounts });
        }

        [HttpPost("CreateNewBooking")]
        public async Task<IActionResult> BookAppointment([FromForm] CreateBookingDTO bookingModel)
        {
            _logger.LogInformation("Action Started");
            await _patientService.CreateNewBookingAsync(bookingModel.appointmentId, bookingModel.PatientId , bookingModel.CouponName);
            _logger.LogInformation("Action Ended");
            return Ok($"Appointment Time With ID: {bookingModel.appointmentId} is sucessuffly Booked!");
        }

        [HttpPost("cancelAppointment")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {

            await _patientService.CancelBookingAsync(bookingId);

            return Ok($"Appointment Time with ID: {bookingId} is Canceled");

           
        }

        [HttpGet("GetPatientSpecificBookings")]
        public async Task<IActionResult> GetPatientSpecificBookings (string patientId)
        {
            _logger.LogInformation("Action Started");
            var patient = await _patientService.GetPatientSpecificBookingsAsync(patientId);
            _logger.LogInformation("Action Ended");
            return Ok(patient);
        }
    }
}

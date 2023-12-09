
using Application.Contracts;
using Application.Services;
using Domain.DTOS;
using Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Veezta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> DoctorLogin([FromBody] LoginDTO model)
        {
            var result = await _doctorService.DoctorLoginAsync(model);
            if (result)
            {
                return Ok("Login Sucessful");
            }

            return Unauthorized("Login Failed");
        }

        [HttpGet("GetSpecificDoctorAppointment")]
        public async Task<IActionResult> GetSpecificDoctorAppointment(int doctorId , [FromBody]PaginationAndSearchDTO request)
        {
            var (appointments,totalcounts) = await _doctorService.GetAppointmentsForDoctorAsync(doctorId , request);
            return Ok(new { appointments, totalcounts });
        }

        [HttpGet("{doctorId}/appointments")]
        public async Task<IActionResult> AddDoctorAppointment([FromBody]AddAppointmentDTO model)
        {
            var appointment = await _doctorService.AddDoctorAppointmentAsync(model);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data , Try Again");
            }
            return Ok(model);
        }

        [HttpDelete("DeleteAppointment")]
        public async Task<IActionResult> DeleteAppointment(int appointmentId)
        {
             await _doctorService.DeleteTimeAppointmentAsync(appointmentId);
            return Ok($"Time ID: {appointmentId} is Deleted");
        }

        [HttpPut("DoctorUpdateAppointment")]
        public async Task<IActionResult>DoctorUpdateAppointment (int timeId , UpdateAppointmentDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Invalid Input , Please Try Again!");
            }

            var result = await _doctorService.DoctorUpdateAppointmentAsync(timeId, model);  
            if(!result)
            {
                return BadRequest($"The id you entered : {timeId} is not found");
            }
            return Ok($"Appointment Updated Sucessfully!\n {model}");
        }

        [HttpPost("ConfirmCheckup")]
        public async Task<IActionResult>CheckupStatus(int bookingId)
        {
            var success = await _doctorService.DoctorConfirmCheckUp(bookingId);
             return Ok("Booking status updated successfully");

        }


    }
}


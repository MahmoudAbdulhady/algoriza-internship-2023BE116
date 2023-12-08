
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
        public async Task<IActionResult> GetSpecificDoctorAppointment(int doctorId)
        {
            var appointments = await _doctorService.GetAppointmentsForDoctorAsync(doctorId);
            if (!appointments.Any())
            {
                return NotFound("No Appointments To Show Doctors didn't Provide any Appointments !");
            }
            return Ok(appointments);
        }

        [HttpPost("AddDoctorAppointment")]
        public async Task<IActionResult> AddDoctorAppointment(AddAppointmentDTO model)
        {
            var appointment = await _doctorService.AddDoctorAppointment(model);
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data , Try Again");
            }
            return Ok(model);
        }

        [HttpDelete("DeleteAppointment")]
        public async Task<IActionResult> DeleteAppointment(int timeId)
        {
        await _doctorService.DeleteTimeAppointmentAsync(timeId);
           
            {
                return NotFound($"The ID: {timeId} is not found");
            }
            return Ok($"Time ID: {timeId} is Deleted");
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


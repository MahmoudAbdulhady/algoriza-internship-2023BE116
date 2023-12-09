﻿
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
        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> PatientRegister([FromForm] PatientRegisterDTO model)
        {
            if(ModelState.IsValid)
            {
                var patient = await _patientService.RegisterPatientAsync(model);
                if(patient)
                {
                    return Ok("Patient Registered Successfully!");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> PatientLogin([FromBody] LoginDTO model)
        {
            if (ModelState.IsValid)
            {
                var patient = await _patientService.LoginPatientAsync(model);
                if (patient)
                {
                    return Ok("Login Successfully!");
                }
            }
            return Unauthorized("Invalid Email or Password , Please Try Again !");
        }

        [HttpGet("GetDoctorAppointments")]
        public async Task<IActionResult> GetDoctorAppointments()
        {
            var appointments = await _patientService.GetAppointmentsForDoctorAsync();
            if (!appointments.Any())
            {
                return NotFound("No Appointments To Show Doctors didn't Provide any Appointments !");
            }
            return Ok(appointments);
        }

        [HttpPost("CreateNewBooking")]
        public async Task<IActionResult> BookAppointment([FromForm] CreateBookingDTO bookingModel)
        {
            await _patientService.CreateNewBooking(bookingModel.appointmentId, bookingModel.PatientId , bookingModel.CouponName);
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
            var patient = await _patientService.GetPatientSpecificBookingsAsync(patientId);
            return Ok(patient);
        }
    }
}

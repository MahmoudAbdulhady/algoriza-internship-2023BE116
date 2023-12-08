
using Application.Contracts;
using Application.Services;
using Domain.DTOS;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Veezta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminServices;
        private readonly UserManager<CustomUser> _userManager;

        public AdminController(IAdminService adminServices , UserManager<CustomUser> userManager)
        {
            _adminServices = adminServices;
            _userManager = userManager;
        }


        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddAddDoctor([FromForm] DoctorRegisterDTO model)
        {
            if (ModelState.IsValid)
            {
                var result = await _adminServices.AddDocotorAsync(model);
                if (result)
                {
                    return Ok("Doctor Registered Successfully !");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetDoctorById")]
        public async Task<IActionResult> GetDoctorById(int doctorId)
        {
            var doctor = await _adminServices.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                return NotFound($"The Doctor Id: {doctorId} not found");
            }

            return Ok(doctor);
        }

        [HttpDelete("DeleteDoctorById")]
        public async Task<IActionResult> DeleteDoctorById(int doctorId)
        {
            var result = await _adminServices.DeleteDoctorAsync(doctorId);
            if (!result)
            {
                return NotFound($"The Doctor Id: {doctorId} not found");
            }
            return Ok($"Doctor With Id: {doctorId} is Deleted !");
        }

        [HttpPut("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctor(int doctorId, [FromBody] DoctorUpdateDTO model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _adminServices.DoctorUpdateAsync(doctorId, model);
            if (!success)
            {
                return NotFound($"Doctor with ID {doctorId} not found.");
            }

            return Ok($"Doctor with ID {doctorId} updated successfully.");
        }

        [HttpPost("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors([FromBody] PaginationAndSearchDTO request)
        {
            var (doctors, totalCount) = await _adminServices.GetAllDoctorsAsync(request);
            return Ok(new { doctors, totalCount });
        }

        [HttpGet("NumOfDoctor")]
        public IActionResult NumOfDoctor()
        {
            return Ok($"Total Number of Doctors = {_adminServices.GetTotalNumOfDoctors()}");
        }

        [HttpGet("TotalNumberOfPatients")]
        public async Task<IActionResult> TotalNumberOfPatients()
        {
            var patients =await _adminServices.TotalNumberOfPatients();
            if(patients == null)
            {
                return NotFound("No Patients Found");
            }
            return Ok($"Total Number Of Patients : {patients}");
        }

        [HttpPost("GetAllPatients")]
        public async Task<IActionResult> GetAllPatients([FromBody] PaginationAndSearchDTO request)
        {
            request.PageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize < 1 ? 10 : request.PageSize;


            var (patients , totalPatients) = await _adminServices.GetAllPatientsAsync(request);
            return Ok(new { patients , totalPatients });
        }

        [HttpGet("GetPatientById")]
        public async Task<IActionResult>GetPatientById (string patientId)
        {
            var patient = await _adminServices.GetPatientByIdAsync(patientId);
            if(patient == null)
            {
                return NotFound($"The Patient with ID: {patientId} not found ! ");
            }
            return Ok(patient);
        }

        [HttpGet("TopFiveSpecalizations")]
        public async Task<IActionResult> TopFiveSpecalizations()
        {
            var topFiveSpecalizations =  await _adminServices.GetTopSpecializationsAsync();
            return Ok(topFiveSpecalizations);
        }

        [HttpGet("TopTenDoctors")]
        public async Task<IActionResult> TopTenDoctors()
        {
            var topTenDoctors = await _adminServices.GetTopTenDoctors();
            return Ok(topTenDoctors);
        }

        [HttpGet("GetNumberOfRequests")]
        public async Task<IActionResult> GetNumberOfRequests()
        {
            var numberOfRequests = await _adminServices.GetNumberOfRequestsAsync();
            return Ok(numberOfRequests);
        }

        [HttpGet("NumOfDoctorsInTheLast24")]
        public async Task<IActionResult> NumOfDoctorsInTheLast24()
        {
            int count = await _adminServices.GetNumberOfDoctorsAddedLast24HoursAsync();
            return Ok(count);
        }

        [HttpPost("sendEmail/{doctorId}")]
        public async Task<IActionResult> SendEmailToDoctor(string doctorId)
        {
            await _adminServices.SendEmailToDoctorAsync(doctorId);
            return Ok("Email sent successfully.");
        }




    }
}

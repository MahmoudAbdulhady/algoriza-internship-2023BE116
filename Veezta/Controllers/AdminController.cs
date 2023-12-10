
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
        private readonly ILogger<AdminController> _logger;

        public AdminController(IAdminService adminServices , UserManager<CustomUser> userManager, ILogger<AdminController> logger)
        {
            _adminServices = adminServices;
            _userManager = userManager;
            _logger = logger;
        }


        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddAddDoctor([FromForm] DoctorRegisterDTO model)
        {
            _logger.LogInformation($"Action Started");

            if (ModelState.IsValid)
            {
                var result = await _adminServices.AddDocotorAsync(model);
                if (result)
                {
                    _logger.LogInformation($"Action Ended ");
                    return Ok("Doctor Registered Successfully !");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetDoctorById")]
        public async Task<IActionResult> GetDoctorById(int doctorId)
        {
            _logger.LogInformation("Action Started");
            var doctor = await _adminServices.GetDoctorByIdAsync(doctorId);
            if (doctor == null)
            {
                return NotFound($"The Doctor Id: {doctorId} not found");
            }

            _logger.LogInformation("Action Ended");
            return Ok(doctor);
        }

        [HttpDelete("DeleteDoctorById")]
        public async Task<IActionResult> DeleteDoctorById(int doctorId)
        {
            _logger.LogInformation("Action Started");
            var result = await _adminServices.DeleteDoctorAsync(doctorId);
            if (!result)
            {
                return NotFound($"The Doctor Id: {doctorId} not found");
            }
            _logger.LogInformation("Action Ended");
            return Ok($"Doctor With Id: {doctorId} is Deleted !");
        }

        [HttpPut("UpdateDoctorInfo")]
        public async Task<IActionResult> UpdateDoctorInfo([FromForm] DoctorUpdateDTO model)
        {
            _logger.LogInformation("Action Started");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var success = await _adminServices.DoctorUpdateAsync(model);
            if (!success)
            {
                return NotFound($"Doctor with ID {model.doctorId} not found.");
            }
            _logger.LogInformation("Action Ended");
            return Ok($"Doctor with ID {model.doctorId} updated successfully.");

        }

        [HttpPost("GetAllDoctors")]
        public async Task<IActionResult> GetAllDoctors([FromBody] PaginationAndSearchDTO request)
        {
            _logger.LogInformation("Action Started");
            var (doctors, totalCount) = await _adminServices.GetAllDoctorsAsync(request);
            _logger.LogInformation("Action Ended");
            return Ok(new { doctors, totalCount });
            
        }

        [HttpGet("NumOfDoctor")]
        public IActionResult NumOfDoctor()
        {
            _logger.LogInformation("Action Started");
            _logger.LogInformation("Action Ended");
            return Ok($"Total Number of Doctors = {_adminServices.GetTotalNumOfDoctors()}");
            
        }

        [HttpGet("TotalNumberOfPatients")]
        public async Task<IActionResult> TotalNumberOfPatients()
        {
            _logger.LogInformation("Action Started");
            var patients =await _adminServices.TotalNumberOfPatients();
            if(patients == null)
            {
                return NotFound("No Patients Found");
            }
            _logger.LogInformation("Action Ended");
            return Ok($"Total Number Of Patients : {patients}");
        }

        [HttpPost("GetAllPatients")]
        public async Task<IActionResult> GetAllPatients([FromBody] PaginationAndSearchDTO request)
        {
            _logger.LogInformation("Action Started");
            request.PageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
            request.PageSize = request.PageSize < 1 ? 10 : request.PageSize;


            var (patients , totalPatients) = await _adminServices.GetAllPatientsAsync(request);
            _logger.LogInformation("Action Ended");
            return Ok(new { patients , totalPatients });
        }

        [HttpGet("GetPatientById")]
        public async Task<IActionResult>GetPatientById (string patientId)
        {
            _logger.LogInformation("Action Started");
            var patient = await _adminServices.GetPatientByIdAsync(patientId);
            if(patient == null)
            {
                return NotFound($"The Patient with ID: {patientId} not found ! ");
            }
            _logger.LogInformation("Action Ended");
            return Ok(patient);
        }

        [HttpGet("TopFiveSpecalizations")]
        public async Task<IActionResult> TopFiveSpecalizations()
        {
            _logger.LogInformation("Action Started");
            var topFiveSpecalizations =  await _adminServices.GetTopSpecializationsAsync();
            _logger.LogInformation("Action Ended");
            return Ok(topFiveSpecalizations);
        }

        [HttpGet("TopTenDoctors")]
        public async Task<IActionResult> TopTenDoctors()
        {
            _logger.LogInformation("Action Started");
            var topTenDoctors = await _adminServices.GetTopTenDoctorsAsync();
            _logger.LogInformation("Action Ended");
            return Ok(topTenDoctors);
        }

        [HttpGet("GetNumberOfRequests")]
        public async Task<IActionResult> GetNumberOfRequests()
        {
            _logger.LogInformation("Action Started");
            var numberOfRequests = await _adminServices.GetNumberOfRequestsAsync();
            _logger.LogInformation("Action Ended");
            return Ok(numberOfRequests);
          
        }

        [HttpGet("NumOfDoctorsInTheLast24")]
        public async Task<IActionResult> NumOfDoctorsInTheLast24()
        {
            _logger.LogInformation("Action Started");
            int count = await _adminServices.GetNumberOfDoctorsAddedLast24HoursAsync();
            _logger.LogInformation("Action Ended");
            return Ok(count);
        }

    }
}

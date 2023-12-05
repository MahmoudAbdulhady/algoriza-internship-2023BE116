
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
    public class AdminController : Controller
    {
        private readonly AdminServices _adminServices;
        private readonly UserManager<CustomUser> _userManager;

        public AdminController(AdminServices adminServices , UserManager<CustomUser> userManager)
        {
            _adminServices = adminServices;
            _userManager = userManager;
        }


        [HttpPost("upload-image")]
        public async Task<IActionResult> UploadImage(IFormFile image)
        {
            if(image == null || image.Length == 0)
            {
                return BadRequest("No File Uploaded");
            }

            // Generate a unique file name to prevent overwriting existing files
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

            // Save the image to the 'wwwroot/images' folder
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Assuming you get the user from the User Identity
            var users = await _userManager.Users.Where(u => u.AccountRole == AccountRole.Doctor).ToListAsync();
            if (users != null)
            {
                foreach (var user in users)
                {
                    user.ImageUrl = fileName;
                    await _userManager.UpdateAsync(user);
                }
            }

            return Ok(new { fileName });


        }

        [HttpPost("AddDoctor")]
        public async Task<IActionResult> AddAddDoctor([FromBody] DoctorRegisterDTO model)
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

        [HttpDelete]
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



    }
}

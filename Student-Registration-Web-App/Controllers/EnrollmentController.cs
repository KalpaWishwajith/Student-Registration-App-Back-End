using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Student_Registration_Web_App.Controllers
{
    [Route("v1/enrollments")]
    [ApiController]
    public class EnrollmentController(IEnrollmentRepository enrollmentRepository) : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository = enrollmentRepository;

        [HttpGet("all")]
        public async Task<ActionResult<List<Enrollment>>> GetEnrollments()
        {
            try
            {
                var enrollments = await _enrollmentRepository.GetEnrollmentsAsync();
                return Ok(new
                {
                    Message = "Enrollments retrieved successfully.",
                    Data = enrollments
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("addEnroll")]
        public async Task<ActionResult<Enrollment>> AddEnrollment(Enrollment enrollment)
        {
            try
            {
                if (enrollment == null)
                {
                    return BadRequest("Enrollment object is null.");
                }

                await _enrollmentRepository.AddEnrollmentAsync(enrollment);
                return CreatedAtAction(nameof(GetEnrollments), new { id = enrollment.EnrollmentID }, enrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete/{enrollmentId}")]
        public async Task<IActionResult> RemoveEnrollment(int enrollmentId)
        {
            try
            {
                await _enrollmentRepository.RemoveEnrollmentAsync(enrollmentId);
                return Ok(new
                {
                    Message = $"Enrollment with ID {enrollmentId} removed successfully.",
                    
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
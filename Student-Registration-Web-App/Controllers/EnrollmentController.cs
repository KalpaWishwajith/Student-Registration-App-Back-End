using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentController(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Enrollment>>> GetEnrollments()
        {
            return await _enrollmentRepository.GetEnrollmentsAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Enrollment>> AddEnrollment(Enrollment enrollment)
        {
            await _enrollmentRepository.AddEnrollmentAsync(enrollment);
            return CreatedAtAction(nameof(GetEnrollments), new { id = enrollment.EnrollmentID }, enrollment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveEnrollment(int id)
        {
            await _enrollmentRepository.RemoveEnrollmentAsync(id);
            return NoContent();
        }
    }
}

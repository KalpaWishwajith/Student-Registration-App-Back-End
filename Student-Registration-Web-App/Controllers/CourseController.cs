using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly ICourseRepository _courseRepository;

        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Course>>> GetCourses()
        {
            return await _courseRepository.GetCoursesAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            var course = await _courseRepository.GetCourseByIdAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return course;
        }

        [HttpPost]
        public async Task<ActionResult<Course>> AddCourse(Course course)
        {
            await _courseRepository.AddCourseAsync(course);
            return CreatedAtAction(nameof(GetCourse), new { id = course.CourseID }, course);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, Course course)
        {
            if (id != course.CourseID)
            {
                return BadRequest();
            }

            await _courseRepository.UpdateCourseAsync(course);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            await _courseRepository.DeleteCourseAsync(id);
            return NoContent();
        }
    }
}

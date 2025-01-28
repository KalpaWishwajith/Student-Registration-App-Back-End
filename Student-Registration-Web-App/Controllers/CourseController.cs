using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Student_Registration_Web_App.Controllers
{
    [Route("v1/courses")]
    [ApiController]
    public class CourseController(ICourseRepository courseRepository) : ControllerBase
    {
        private readonly ICourseRepository _courseRepository = courseRepository;

        [HttpGet("all")]
        public async Task<ActionResult<List<Course>>> GetCourses()
        {
            try
            {
                var courses = await _courseRepository.GetCoursesAsync();
                return Ok(new
                {
                    Message = "Courses retrieved successfully.",
                    Data = courses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{courseId}")]
        public async Task<ActionResult<Course>> GetCourse(int courseId)
        {
            try
            {
                var course = await _courseRepository.GetCourseByIdAsync(courseId);
                if (course == null)
                {
                    return NotFound($"Course with ID {courseId} not found.");
                }
                return Ok(new
                {
                    Message = $"Course with ID {courseId} retrieved successfully.",
                    Data = course
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("addCourse")]
        public async Task<ActionResult<Course>> AddCourse(Course course)
        {
            try
            {
                if (course == null)
                {
                    return BadRequest("Course object is null.");
                }

                await _courseRepository.AddCourseAsync(course);
                return CreatedAtAction(nameof(GetCourse), new { id = course.CourseID }, course);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update/{courseId}")]
        public async Task<IActionResult> UpdateCourse(int courseId, Course course)
        {
            try
            {
                if (courseId != course.CourseID)
                {
                    return BadRequest("Course ID mismatch.");
                }

                var existingCourse = await _courseRepository.GetCourseByIdAsync(courseId);
                if (existingCourse == null)
                {
                    return NotFound($"Course with ID {courseId} not found.");
                }

                await _courseRepository.UpdateCourseAsync(course);
                return Ok(new
                {
                    Message = "Course updated successfully.",
                    UpdatedCourse = course
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete/{courseId}")]
        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            try
            {
                var courseToDelete = await _courseRepository.GetCourseByIdAsync(courseId);
                if (courseToDelete == null)
                {
                    return NotFound($"Course with ID {courseId} not found.");
                }

                await _courseRepository.DeleteCourseAsync(courseId);
                return Ok(new
                {
                    Message = "Course deleted successfully.",
                    DeletedCourse = courseToDelete
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
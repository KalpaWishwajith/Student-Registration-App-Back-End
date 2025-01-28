using Microsoft.AspNetCore.Mvc;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using Student_Registration_Web_App.Repositories;

namespace Student_Registration_Web_App.Controllers
{
    [Route("v1/students")] // Base route for the controller
    [ApiController]
    public class StudentController(IStudentRepository studentRepository) : ControllerBase
    {
        private readonly IStudentRepository _studentRepository = studentRepository;

        [HttpGet("all")] // Change route to "api/students/all"
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            try
            {
                var students = await _studentRepository.GetStudentsAsync();
                return Ok(new
                {
                    Message = "Students retrieved successfully.",
                    Data = students
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<Student>> GetStudent(int studentId)
        {
            try
            {
                var student = await _studentRepository.GetStudentByIdAsync(studentId);
                if (student == null)
                {
                    return NotFound();
                }
                return Ok(new
                {
                    Message = $"Student with ID {studentId} retreived successfully.",
                    Data = student
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<Student>> AddStudent(Student student)
        {
            try
            {
                if (student == null)
                {
                    return BadRequest("Student object is null.");
                }

                await _studentRepository.AddStudentAsync(student);
                return CreatedAtAction(nameof(GetStudent), new { id = student.StudentID }, student);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update/{studentId}")]
        public async Task<IActionResult> UpdateStudent(int studentId, Student student)
        {
            try
            {
                if (student == null || studentId != student.StudentID)
                {
                    return BadRequest("Invalid student data.");
                }

                var existingStudent = await _studentRepository.GetStudentByIdAsync(studentId);
                if (existingStudent == null)
                {
                    return NotFound($"Student with ID {studentId} not found.");
                }

                await _studentRepository.UpdateStudentAsync(student);
                return Ok(new
                {
                    Message = "Student updated successfully.",
                    UpdatedStudent = student
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete/{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            try
            {
                var studentToDelete = await _studentRepository.GetStudentByIdAsync(studentId);
                if (studentToDelete == null)
                {
                    return NotFound($"Student with ID {studentId} not found.");
                }
                await _studentRepository.DeleteStudentAsync(studentId);
                return Ok(new
                {
                    Message = "Student deleted successfully.",
                    DeletedStudent = studentToDelete
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("enrolled-courses/{studentId}")]
        public async Task<ActionResult<List<Course>>> GetEnrolledCoursesByStudent(int studentId)
        {
            try
            {

                var studentToCheck = await _studentRepository.GetStudentByIdAsync(studentId);
                if (studentToCheck == null)
                {
                    return NotFound($"Student with ID {studentId} not found.");
                }

                var courses = await _studentRepository.GetEnrolledCoursesByStudentAsync(studentId);
                if (courses == null || courses.Count == 0)
                {
                    return NotFound($"No courses found for StudentID: {studentId}");
                }

                return Ok(new
                {
                    Message = $"Enrolled Courses by student with ID {studentId} retrieved successfully.",
                    Data = courses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        // GET: api/student/all-students-with-courses
        [HttpGet("all-students-with-courses")]
        public async Task<ActionResult<Dictionary<Student, List<Course>>>> GetAllStudentsWithCourses()
        {
            try
            {
                var studentsWithCourses = await _studentRepository.GetAllStudentsWithCoursesAsync();
                if (studentsWithCourses == null || studentsWithCourses.Count == 0)
                {
                    return NotFound("No students with enrolled courses found.");
                }

                return Ok(new
                {
                    Message = "Student with courses retrieved successfully.",
                    Data = studentsWithCourses
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}


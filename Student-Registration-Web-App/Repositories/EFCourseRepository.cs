using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Student_Registration_Web_App.Repositories
{
    public class EFCourseRepository : ICourseRepository
    {
        private readonly EFDbContext _context;

        public EFCourseRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<List<Course>> GetCoursesAsync()
        {
            try
            {
                return await _context.Courses.FromSqlRaw("EXEC GetCourses").ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework like Serilog or NLog)
                throw new ApplicationException("An error occurred while retrieving courses.", ex);
            }
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            try
            {
                var course = _context.Courses
                    .FromSqlRaw("EXEC GetCourseById @CourseID", new SqlParameter("@CourseID", courseId))
                    .AsEnumerable() 
                    .FirstOrDefault();

                if (course == null)
                {
                    throw new KeyNotFoundException($"Student with ID {courseId} not found.");
                }
                return course;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while retrieving the course by ID.", ex);
            }
        }

        public async Task AddCourseAsync(Course course)
        {
            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course), "Course object cannot be null.");
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddCourse @CourseName, @CourseDescription",
                    new SqlParameter("@CourseName", course.CourseName),
                    new SqlParameter("@CourseDescription", course.CourseDescription));
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while adding the course.", ex);
            }
        }

        public async Task UpdateCourseAsync(Course course)
        {
            try
            {
                if (course == null)
                {
                    throw new ArgumentNullException(nameof(course), "Course object cannot be null.");
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateCourse @CourseID, @CourseName, @CourseDescription",
                    new SqlParameter("@CourseID", course.CourseID),
                    new SqlParameter("@CourseName", course.CourseName),
                    new SqlParameter("@CourseDescription", course.CourseDescription));
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while updating the course.", ex);
            }
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteCourse @CourseID",
                    new SqlParameter("@CourseID", courseId));
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while deleting the course.", ex);
            }
        }
    }
}
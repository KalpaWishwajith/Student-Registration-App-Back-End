using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;

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
            return await _context.Courses.FromSqlRaw("EXEC GetCourses").ToListAsync();
        }

        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            return await _context.Courses.FromSqlRaw("EXEC GetCourseById @CourseID", new SqlParameter("@CourseID", courseId)).FirstOrDefaultAsync();
        }

        public async Task AddCourseAsync(Course course)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC AddCourse @CourseName, @CourseDescription",
                new SqlParameter("@CourseName", course.CourseName),
                new SqlParameter("@CourseDescription", course.CourseDescription));
        }

        public async Task UpdateCourseAsync(Course course)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateCourse @CourseID, @CourseName, @CourseDescription",
                new SqlParameter("@CourseID", course.CourseID),
                new SqlParameter("@CourseName", course.CourseName),
                new SqlParameter("@CourseDescription", course.CourseDescription));
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteCourse @CourseID",
                new SqlParameter("@CourseID", courseId));
        }
    }
}

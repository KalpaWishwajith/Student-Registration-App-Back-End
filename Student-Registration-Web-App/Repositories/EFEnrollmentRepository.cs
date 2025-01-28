using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Repositories
{
    public class EFEnrollmentRepository : IEnrollmentRepository
    {
        private readonly EFDbContext _context;

        public EFEnrollmentRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<List<Enrollment>> GetEnrollmentsAsync()
        {
            return await _context.Enrollments.FromSqlRaw("EXEC GetEnrollments").ToListAsync();
        }

        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC AddEnrollment @CourseID, @StudentID",
                new SqlParameter("@CourseID", enrollment.CourseID),
                new SqlParameter("@StudentID", enrollment.StudentID));
        }

        public async Task RemoveEnrollmentAsync(int enrollmentId)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC RemoveEnrollment @EnrollmentID",
                new SqlParameter("@EnrollmentID", enrollmentId));
        }
    }
}

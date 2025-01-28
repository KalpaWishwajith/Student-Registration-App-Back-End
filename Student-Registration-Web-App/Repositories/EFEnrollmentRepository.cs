using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                return await _context.Enrollments.FromSqlRaw("EXEC GetEnrollments").ToListAsync();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("An error occurred while retrieving enrollments.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while retrieving enrollments.", ex);
            }
        }

        public async Task AddEnrollmentAsync(Enrollment enrollment)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC AddEnrollment @CourseID, @StudentID",
                    new SqlParameter("@CourseID", enrollment.CourseID),
                    new SqlParameter("@StudentID", enrollment.StudentID));
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("An error occurred while adding the enrollment.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while adding the enrollment.", ex);
            }
        }

        public async Task RemoveEnrollmentAsync(int enrollmentId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("EXEC RemoveEnrollment @EnrollmentID",
                    new SqlParameter("@EnrollmentID", enrollmentId));
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("An error occurred while removing the enrollment.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while removing the enrollment.", ex);
            }
        }
    }
}
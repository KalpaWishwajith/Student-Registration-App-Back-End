using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Repositories
{
    public class EFStudentRepository : IStudentRepository
    {
        private readonly EFDbContext _context;

        public EFStudentRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentsAsync()
        {
            return await _context.Students.FromSqlRaw("EXEC GetStudents").ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
        {
            return await _context.Students.FromSqlRaw("EXEC GetStudentById @StudentID", new SqlParameter("@StudentID", studentId)).FirstOrDefaultAsync();
        }

        public async Task AddStudentAsync(Student student)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC AddStudent @FirstName, @LastName, @Email, @PasswordHash",
                new SqlParameter("@FirstName", student.FirstName),
                new SqlParameter("@LastName", student.LastName),
                new SqlParameter("@Email", student.Email),
                new SqlParameter("@PasswordHash", student.PasswordHash));
        }

        public async Task UpdateStudentAsync(Student student)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateStudent @StudentID, @FirstName, @LastName, @Email",
                new SqlParameter("@StudentID", student.StudentID),
                new SqlParameter("@FirstName", student.FirstName),
                new SqlParameter("@LastName", student.LastName),
                new SqlParameter("@Email", student.Email));
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteStudent @StudentID",
                new SqlParameter("@StudentID", studentId));
        }
    }
}

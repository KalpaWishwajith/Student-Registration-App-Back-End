using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            try
            {
                return await _context.Students.FromSqlRaw("EXEC GetStudents").ToListAsync();
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("An SQL error occurred while retrieving students.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while retrieving students.", ex);
            }
        }

        public async Task<Student> GetStudentByIdAsync(int studentId)
{
    try
    {
                var student = _context.Students
                    .FromSqlRaw("EXEC GetStudentById @StudentID", new SqlParameter("@StudentID", studentId))
                    .AsEnumerable() // Perform further operations on the client side
                    .FirstOrDefault();

                if (student == null)
        {
            throw new KeyNotFoundException($"Student with ID {studentId} not found.");
        }

        return student;
    }
    catch (SqlException ex)
    {
        throw new ApplicationException("An error occurred while retrieving the student by ID.", ex);
    }
    catch (Exception ex)
    {
        throw new ApplicationException("An unexpected error occurred while retrieving the student by ID.", ex);
    }
}

        public async Task AddStudentAsync(Student student)
        {
            try
            {
                if (student == null)
                {
                    throw new ArgumentNullException(nameof(student), "Student object cannot be null.");
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddStudent @FirstName, @LastName, @Email, @PasswordHash",
                    new SqlParameter("@FirstName", student.FirstName),
                    new SqlParameter("@LastName", student.LastName),
                    new SqlParameter("@Email", student.Email),
                    new SqlParameter("@PasswordHash", student.PasswordHash));
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("An error occurred while adding the student.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while adding the student.", ex);
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                if (student == null)
                {
                    throw new ArgumentNullException(nameof(student), "Student object cannot be null.");
                }

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateStudent @StudentID, @FirstName, @LastName, @Email, @PasswordHash",
                    new SqlParameter("@StudentID", student.StudentID),
                    new SqlParameter("@FirstName", student.FirstName),
                    new SqlParameter("@LastName", student.LastName),
                    new SqlParameter("@Email", student.Email),
                    new SqlParameter("@PasswordHash", student.PasswordHash));
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("An error occurred while updating the student.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while updating the student.", ex);
            }
        }

        public async Task DeleteStudentAsync(int studentId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteStudent @StudentID",
                    new SqlParameter("@StudentID", studentId));
            }
            catch (SqlException ex)
            {
                throw new ApplicationException("An error occurred while deleting the student.", ex);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred while deleting the student.", ex);
            }
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using System;
using System.Threading.Tasks;

namespace Student_Registration_Web_App.Repositories
{
    public class EFAdminRepository : IAdminRepository
    {
        private readonly EFDbContext _context;

        public EFAdminRepository(EFDbContext context)
        {
            _context = context;
        }

        public async Task<Admin> AdminLoginAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    throw new ArgumentException("Username and password are required.");
                }


                var admin = _context.Admins
                    .FromSqlRaw("EXEC LoginAdmin @Username, @PasswordHash", new SqlParameter("@Username", username), new SqlParameter("@PasswordHash", password))
                    .AsEnumerable()
                    .FirstOrDefault();

                if (admin == null)
                {
                    throw new InvalidOperationException("Invalid username or password.");
                }

                
               return admin;
                

                
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An unexpected error occurred.", ex);
            }
        }

        public async Task<Admin> GetAdminByUsernameAsync(string username)
        {
            try
            {
                var admin =   _context.Admins
                    .FromSqlRaw("EXEC GetAdminByUsername @Username", new SqlParameter("@Username", username))
                    .AsEnumerable()
                    .FirstOrDefault();

                if (admin == null)
                {
                    throw new KeyNotFoundException($"Student with username {username} not found.");
                }

                return admin;
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework like Serilog or NLog)
                throw new ApplicationException("An error occurred while retrieving the admin by username.", ex);
            }
        }

        public async Task<Admin> GetAdminByIdAsync(int adminId)
        {
            try
            {
                var admin=   _context.Admins
                    .FromSqlRaw("EXEC GetAdminById @AdminID", new SqlParameter("@AdminID", adminId))
                    .AsEnumerable()
                    .FirstOrDefault();

                if (admin == null)
                {
                    throw new KeyNotFoundException($"Student with ID {adminId} not found.");
                    
                }

                    return admin;
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while retrieving the admin by ID.", ex);
            }
        }

        public async Task AddAdminAsync(Admin admin)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC AddAdmin @Username, @PasswordHash",
                    new SqlParameter("@Username", admin.Username),
                    new SqlParameter("@PasswordHash", admin.PasswordHash));
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while adding the admin.", ex);
            }
        }

        public async Task UpdateAdminAsync(Admin admin)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC UpdateAdmin @AdminID, @Username, @PasswordHash, @Email",
                    new SqlParameter("@AdminID", admin.AdminID),
                    new SqlParameter("@Username", admin.Username),
                    new SqlParameter("@PasswordHash", admin.PasswordHash));
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while updating the admin.", ex);
            }
        }

        public async Task DeleteAdminAsync(int adminId)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC DeleteAdmin @AdminID",
                    new SqlParameter("@AdminID", adminId));
            }
            catch (Exception ex)
            {
                // Log the exception
                throw new ApplicationException("An error occurred while deleting the admin.", ex);
            }
        }
    }
}
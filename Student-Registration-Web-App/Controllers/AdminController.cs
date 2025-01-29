using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using Student_Registration_Web_App.Repositories;
using System;
using System.Threading.Tasks;

namespace Student_Registration_Web_App.Controllers
{
    [Route("v1/admins")]
    [ApiController]
    public class AdminController(IAdminRepository adminRepository) : ControllerBase
    {
        private readonly IAdminRepository _adminRepository = adminRepository;

        [HttpPost("login")]
        public async Task<ActionResult<Admin>> AdminLogin(AdminLoginRequest loginRequest)
        {
            try
            {
                if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
                {
                    return BadRequest("Username and password are required.");
                }

                var admin = await _adminRepository.AdminLoginAsync(loginRequest.Username, loginRequest.Password);
                if (admin == null)
                {
                    return Unauthorized("Invalid username or password.");
                }

                return Ok(new
                {
                    Message = "Admin login successful.",
                    Data = admin,
                    Role = "Admin"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetById/{adminId}")]
        public async Task<ActionResult<Admin>> GetAdminById(int adminId)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByIdAsync(adminId);
                if (admin == null)
                {
                    return NotFound($"Admin with ID {adminId} not found.");
                }
                return Ok(new
                {
                    Message = $"Admin with username {adminId} retrieved successfully.",
                    Data = admin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("GetByUsername{username}")]
        public async Task<ActionResult<Admin>> GetAdmin(string username)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByUsernameAsync(username);
                if (admin == null)
                {
                    return NotFound($"Admin with username {username} not found.");
                }
                return Ok(new
                {
                    Message = $"Admin with username {username} retrieved successfully.",
                    Data = admin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<Admin>> AddAdmin(Admin admin)
        {
            try
            {
                if (admin == null)
                {
                    return BadRequest("Admin object is null.");
                }

                await _adminRepository.AddAdminAsync(admin);
                return Ok(new
                {
                    Message = "Admin created successfully.",
                    Data = admin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPut("update/{adminId}")]
        public async Task<IActionResult> UpdateAdmin(int adminId, Admin admin)
        {
            try
            {
                if (adminId != admin.AdminID)
                {
                    return BadRequest("Admin ID mismatch.");
                }

                var adminToUpdate = await _adminRepository.GetAdminByUsernameAsync(admin.Username);
                if (adminToUpdate == null)
                {
                    return NotFound($"Admin with ID {adminId} not found.");
                }

                await _adminRepository.UpdateAdminAsync(admin);
                return Ok(new
                {
                    Message = "Admin updated successfully.",
                    UpdatedAdmin = admin
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("delete/{adminId}")]
        public async Task<IActionResult> DeleteAdmin(int adminId)
        {
            try
            {
                var adminToDelete = await _adminRepository.GetAdminByIdAsync(adminId);
                if (adminToDelete == null)
                {
                    return NotFound($"Admin with ID {adminId} not found.");
                }

                await _adminRepository.DeleteAdminAsync(adminId);
                return Ok(new
                {
                    Message = "Admin deleted successfully.",
                    DeletedAdmin = adminToDelete
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred: {ex.Message}");
            }
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student_Registration_Web_App.Contracts;
using Student_Registration_Web_App.EntityModels;
using System;
using System.Threading.Tasks;

namespace Student_Registration_Web_App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        [HttpGet("ById/{id}")]
        public async Task<ActionResult<Admin>> GetAdminById(int id)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByIdAsync(id);
                if (admin == null)
                {
                    return NotFound($"Admin with ID {id} not found.");
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<Admin>> GetAdmin(string username)
        {
            try
            {
                var admin = await _adminRepository.GetAdminByUsernameAsync(username);
                if (admin == null)
                {
                    return NotFound($"Admin with username {username} not found.");
                }
                return Ok(admin);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Admin>> AddAdmin(Admin admin)
        {
            try
            {
                if (admin == null)
                {
                    return BadRequest("Admin object is null.");
                }

                await _adminRepository.AddAdminAsync(admin);
                return CreatedAtAction(nameof(GetAdmin), new { username = admin.Username }, admin);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new admin record.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, Admin admin)
        {
            try
            {
                if (id != admin.AdminID)
                {
                    return BadRequest("Admin ID mismatch.");
                }

                var adminToUpdate = await _adminRepository.GetAdminByUsernameAsync(admin.Username);
                if (adminToUpdate == null)
                {
                    return NotFound($"Admin with ID {id} not found.");
                }

                await _adminRepository.UpdateAdminAsync(admin);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating admin record.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            try
            {
                var adminToDelete = await _adminRepository.GetAdminByIdAsync(id);
                if (adminToDelete == null)
                {
                    return NotFound($"Admin with ID {id} not found.");
                }

                await _adminRepository.DeleteAdminAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting admin record.");
            }
        }
    }
}
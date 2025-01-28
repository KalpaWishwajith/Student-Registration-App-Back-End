using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Contracts
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByUsernameAsync(string username);
        Task AddAdminAsync(Admin admin);
        Task UpdateAdminAsync(Admin admin);
        Task DeleteAdminAsync(int adminId);
        Task GetAdminByIdAsync(int id);
    }
}

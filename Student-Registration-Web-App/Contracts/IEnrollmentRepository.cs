using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Contracts
{
    public interface IEnrollmentRepository
    {
        Task<List<Enrollment>> GetEnrollmentsAsync();
        Task AddEnrollmentAsync(Enrollment enrollment);
        Task RemoveEnrollmentAsync(int enrollmentId);
    }
}

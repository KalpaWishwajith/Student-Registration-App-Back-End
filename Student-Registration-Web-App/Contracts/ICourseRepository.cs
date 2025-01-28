using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Contracts
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetCoursesAsync();
        Task<Course> GetCourseByIdAsync(int courseId);
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int courseId);
    }
}

using Student_Registration_Web_App.EntityModels;

namespace Student_Registration_Web_App.Contracts
{
    public interface IStudentRepository
    {
        Task<List<Student>> GetStudentsAsync();
        Task<Student> GetStudentByIdAsync(int studentId);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(Student student);
        Task DeleteStudentAsync(int studentId);
        Task<List<Course>> GetEnrolledCoursesByStudentAsync(int studentId);
        Task<List<StudentCourseResult>> GetAllStudentsWithCoursesAsync();
    }
}

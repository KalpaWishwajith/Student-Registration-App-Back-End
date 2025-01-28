using System.ComponentModel.DataAnnotations;

namespace Student_Registration_Web_App.EntityModels
{
    public class StudentCourseResult
    {
        [Key]
        public int StudentID { get; set; }
        public string StudentName { get; set; }
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
    }
}

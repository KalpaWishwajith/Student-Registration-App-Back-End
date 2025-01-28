using System.ComponentModel.DataAnnotations;

namespace Student_Registration_Web_App.EntityModels
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public DateTime EnrolledDate { get; set; }
        public DateTime? UnEnrolledDate { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}

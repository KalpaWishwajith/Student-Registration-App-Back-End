using System.ComponentModel.DataAnnotations;

namespace Student_Registration_Web_App.EntityModels
{
    public class Course
    {
        [Key]
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public string CourseDescription { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set;}
        public DateTime? DeletedDate { get; set; }



    }
}

using System.ComponentModel.DataAnnotations;

namespace dotNET_courseproject_CourseRegister.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required, StringLength(100)]
        public string CourseName { get; set; }

        [Required, StringLength(100)]
        public string TeacherName { get; set; }

        [Required]
        public DateTime StartedTime { get; set; }

        [Required]
        public int Cost { get; set; }

        [Required]
        public int MaxStudents { get; set; }
        [Required]
        public int CurrentStudents { get; set; }

        [Required]
        public int Duration { get; set; }

        public ICollection<User_Course>? EnrolledUsers { get; set; }
    }
}

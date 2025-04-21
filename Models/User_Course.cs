using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotNET_courseproject_CourseRegister.Models
{
    public class User_Course
    {
        [Key]
        public int UserCourseId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CourseId { get; set; }
        [Required]
        public DateTime RegistedTime { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}

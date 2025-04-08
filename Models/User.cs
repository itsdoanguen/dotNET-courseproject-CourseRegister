using System.ComponentModel.DataAnnotations;

namespace dotNET_courseproject_CourseRegister.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required, StringLength(100)]
        public string UserName { get; set; }
        [Required, StringLength(100)]
        public string Password { get; set; }
        [Required, StringLength(100)]
        public string FullName { get; set; }
        [Required, StringLength(100)]
        public string Email { get; set; }
        [Required, StringLength(100)]
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        [Required]
        public UserRole Role { get; set; } = UserRole.Student;


        public enum UserRole
        {
            Admin,
            Student
        }
    }
}

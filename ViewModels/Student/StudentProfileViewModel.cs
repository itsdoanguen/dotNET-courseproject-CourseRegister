using System.ComponentModel.DataAnnotations;

namespace dotNET_courseproject_CourseRegister.ViewModels.Student
{
    public class StudentProfileViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [Display(Name = "Họ và tên")]
        public string? FullName { get; set; }

        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        [Display(Name = "Ngày tham gia")]
        public DateTime CreatedTime { get; set; }

        [Display(Name = "Số dư tài khoản")]
        public double Money { get; set; }

        [Display(Name = "Vai trò")]
        public string Role { get; set; } 

        [Display(Name = "Số khóa học đã đăng ký")]
        public int TotalEnrolledCourses { get; set; }
    }
}

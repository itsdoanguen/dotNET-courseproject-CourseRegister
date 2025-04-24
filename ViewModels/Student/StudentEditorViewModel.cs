using System.ComponentModel.DataAnnotations;
using dotNET_courseproject_CourseRegister.Attributes;

namespace dotNET_courseproject_CourseRegister.ViewModels.Student
{
    public class StudentEditorViewModel
    {
        [Display(Name = "Họ tên")]
        public string? FullName { get; set; }

        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Năm sinh")]
        public DateTime? DOB { get; set; }
        [EmailAddress]
        public string? Email { get; set; }

        [Display(Name = "Tên tài khoản")]
        public string? UserName { get; set; }

        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }

        [Display(Name = "Nhập lại mật khẩu")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Mật khẩu cũ")]
        public string? OldPassword { get; set; }

        [Display(Name = "Mật khẩu mới")]
        [DataType(DataType.Password)]
        [PasswordComplexity]
        public string? NewPassword { get; set; }

        [Display(Name = "Xác nhận mật khẩu mới")]
        public string? ConfirmNewPassword { get; set; }

    }
}

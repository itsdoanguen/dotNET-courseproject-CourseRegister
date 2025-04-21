using System.ComponentModel.DataAnnotations;
using dotNET_courseproject_CourseRegister.Attributes;

namespace dotNET_courseproject_CourseRegister.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên tài khoản.")]
        [Display(Name = "Tên tài khoản")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Tên tài khoản phải từ 3 đến 100 ký tự.")]
        [DataType(DataType.Text)]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật Khẩu")]
        [PasswordComplexity] 
        public string Password { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu.")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp!")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ và tên.")]
        [Display(Name = "Họ và tên")]
        [StringLength(100, ErrorMessage = "Họ và tên tối đa 100 ký tự.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(15, ErrorMessage = "Số điện thoại tối đa 15 số.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày sinh.")]
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }
    }
}

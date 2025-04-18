using System.ComponentModel.DataAnnotations;

namespace dotNET_courseproject_CourseRegister.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Tên tài khoản hoặc email")]
        [DataType(DataType.Text)]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}

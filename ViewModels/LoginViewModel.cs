using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.ViewModels
{
    public class LoginViewModel : Controller
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

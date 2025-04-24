using Microsoft.AspNetCore.Identity;
using static dotNET_courseproject_CourseRegister.Models.User;

namespace dotNET_courseproject_CourseRegister.ViewModels.Admin
{
    public class AdminManageUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DOB { get; set; }
        public DateTime CreatedTime { get; set; }
        public UserRole UserRole { get; set; }
        public int TotalCourses { get; set; }
    }
}

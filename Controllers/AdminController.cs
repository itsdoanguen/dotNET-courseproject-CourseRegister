using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET: Admin/Dashboard
        //GET: Admin/Reports

        //GET: Admin/ManageCourses
        [HttpGet]
        public IActionResult ManageCourses()
        {
            var courseList = GetCourseList();
            return View(courseList);
        }

        private List<AdminManageCourseViewModel> GetCourseList()
        {
            var courses = _context.Courses.ToList();
            var courseList = new List<AdminManageCourseViewModel>();

            foreach (var course in courses)
            {
                var courseViewModel = new AdminManageCourseViewModel
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    TeacherName = course.TeacherName,
                    StartedTime = course.StartedTime,
                    Cost = course.Cost,
                    MaxStudents = course.MaxStudents,
                    CurrentStudents = course.CurrentStudents,
                    Duration = course.Duration,
                    Status = (AdminManageCourseViewModel.CourseStatus)course.Status
                };
                courseList.Add(courseViewModel);
            }

            return courseList;
        }


        //GET: Admin/ManageUsers
        [HttpGet]
        public IActionResult ManageUsers()
        {
            var usersList = GetUserList();
            return View(usersList);
        }

        private List<AdminManageUserViewModel> GetUserList()
        {
            var users = _context.Users.ToList();
            var userList = new List<AdminManageUserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new AdminManageUserViewModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DOB = user.DOB,
                    CreatedTime = user.CreatedTime,
                    TotalCourses = _context.UserCourses.Count(uc => uc.UserId == user.UserId)
                };
                userList.Add(userViewModel);
            }
            return userList;
        }
    }
}

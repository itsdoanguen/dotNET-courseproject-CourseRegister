using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.ViewModels.Admin;
using dotNET_courseproject_CourseRegister.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET: Admin/Index
        public IActionResult Index()
        {
            return View();
        }

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
        //GET: Admin/ViewUser
        [HttpGet]
        public IActionResult ViewUser(int id)
        {
            var user = GetUserList().FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        //POST: Admin/ResetPassword
        [HttpPost]
        public IActionResult ResetPassword(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Role == Models.User.UserRole.Admin)
            {
                TempData["ErrorMessage"] = $"Không thể đặt lại mật khẩu cho tài khoản Admin!";
                return RedirectToAction("ViewUser", new { id });
            }


            string newPassword = GenerateRandomPassword();
            user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Mật khẩu đã được đặt lại thành công: {newPassword}";
            return RedirectToAction("ViewUser",new {id});
        }
        //POST: Admin/ManageStatus
        [HttpPost]
        public IActionResult ManageStatus(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }
            if (user.Role == Models.User.UserRole.Admin)
            {
                TempData["ErrorMessage"] = $"Không thể thay đổi trạng thái tài khoản Admin!";
                return RedirectToAction("ViewUser", new { id });
            }

            user.IsActive = !user.IsActive;
            _context.SaveChanges();

            TempData["SuccessMessage"] = $"Trạng thái tài khoản đã được cập nhật thành công!";
            return RedirectToAction("ViewUser", new { id });
        }
        private string GenerateRandomPassword(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private List<AdminManageUserViewModel> GetUserList()
        {
            var users = _context.Users.ToList();
            var userList = new List<AdminManageUserViewModel>();
            foreach (var user in users)
            {
                if (user.Role == Models.User.UserRole.Admin)
                {
                    continue;
                }

                var userViewModel = new AdminManageUserViewModel
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    DOB = user.DOB,
                    CreatedTime = user.CreatedTime,
                    UserRole = user.Role,
                    TotalCourses = _context.UserCourses.Count(uc => uc.UserId == user.UserId),
                    IsActive = user.IsActive
                };
                userList.Add(userViewModel);
            }
            return userList;
        }


        //GET: Admin/Dashboard
        [HttpGet]
        public IActionResult Dashboard()
        {
            var dashboardViewModel = new AdminDashboardViewModel();

            var userContext = _context.Users.ToList();
            var courseContext = _context.Courses.ToList();
            var userCourseContext = _context.UserCourses.ToList();

            //Users
            dashboardViewModel.TotalUsers = userContext.Where(u => u.Role == Models.User.UserRole.Student).Count();

            //Courses
            dashboardViewModel.TotalCourses = courseContext.Count();
            dashboardViewModel.TotalActiveCourses = courseContext.Count(c => c.Status == Course.CourseStatus.Active);
            dashboardViewModel.TotalInactiveCourses = courseContext.Count(c => c.Status == Course.CourseStatus.Inactive);
            //Enrolled Users
            dashboardViewModel.TotalEnrolledUsers = userCourseContext.Select(uc => uc.UserId).Distinct().Count();

            //Revenue
            foreach (var item in userCourseContext)
            {
                var course = courseContext.FirstOrDefault(c => c.CourseId == item.CourseId);
                if (course != null)
                {
                    dashboardViewModel.TotalRevenue += course.Cost;
                }
            }

            //Most Enrolling User
            var mostEnrollingUsers = userCourseContext
                .GroupBy(uc => uc.UserId)
                .Select(g => new EnrolledUserViewModel
                {
                    UserName = userContext.FirstOrDefault(u => u.UserId == g.Key)?.UserName,
                    Email = userContext.FirstOrDefault(u => u.UserId == g.Key)?.Email,
                    TotalCourse = g.Count()
                })
                .OrderByDescending(u => u.TotalCourse)
                .Take(5)
                .ToList();
            //Most Enrolled Courses
            var mostEnrolledCourses = userCourseContext
                .GroupBy(uc => uc.CourseId)
                .Select(g => new CourseDashboardViewModel
                {
                    CourseName = courseContext.FirstOrDefault(c => c.CourseId == g.Key)?.CourseName,
                    TotalEnrolledUsers = g.Count()
                })
                .OrderByDescending(c => c.TotalEnrolledUsers)
                .Take(5)
                .ToList();
            dashboardViewModel.MostEnrollingUser = mostEnrollingUsers;
            dashboardViewModel.MostEnrolledCourses = mostEnrolledCourses;


            //Revenue
            foreach (var course in courseContext)
            {
                var courseRevenue = new CourseDashboardViewModel
                {
                    CourseName = course.CourseName,
                    TotalEnrolledUsers = userCourseContext.Count(uc => uc.CourseId == course.CourseId),
                    CourseRevenue = userCourseContext.Where(uc => uc.CourseId == course.CourseId).Sum(uc => _context.Courses.FirstOrDefault(c => c.CourseId == uc.CourseId).Cost)
                };
                dashboardViewModel.AllCourseRevenue.Add(courseRevenue);
            }

            return View(dashboardViewModel);
        }

        //GET: Admin/Reports
        public IActionResult Reports(DateTime? fromD, DateTime? toD)
        {
            var fromDate = fromD?.Date ?? DateTime.Now.AddDays(-30).Date;
            var toDate = (toD?.Date.AddDays(1).AddSeconds(-1)) ?? DateTime.Now.Date.AddDays(1).AddSeconds(-1);


            ViewBag.FromDate = fromDate.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.ToString("yyyy-MM-dd");

            var reportViewModel = new AdminReportViewModel();

            //User
            var newUsers = _context.Users
                .Where(u => u.CreatedTime >= fromDate && u.CreatedTime <= toDate)
                .GroupBy(u => u.CreatedTime.Date)
                .Select(g => new UserPerDay
                {
                    Date = g.Key,
                    Count = g.Count()
                }).ToList();
            reportViewModel.TotalNewUsers = newUsers.Sum(u => u.Count);
            reportViewModel.NewUsersPerDay = newUsers;


            //Erroled
            var erroledUsers = _context.UserCourses
                .Where(u => u.RegistedTime >= fromDate && u.RegistedTime <= toDate)
                .GroupBy(u => u.RegistedTime.Date)
                .Select(g => new ErroledPerDay
                {
                    Date = g.Key,
                    Count = g.Count()
                }).ToList();
            reportViewModel.TotalErroled = erroledUsers.Sum(u => u.Count);
            reportViewModel.ErroledPerDays = erroledUsers;

            //Revenue
            var revenue = _context.UserCourses
                .Where(uc => uc.RegistedTime >= fromDate && uc.RegistedTime <= toDate)
                .GroupBy(uc => uc.RegistedTime.Date)
                .Select(g => new RevenuePerDay
                {
                    Date = g.Key,
                    Revenue = g.Sum(uc => _context.Courses.FirstOrDefault(c => c.CourseId == uc.CourseId).Cost)
                }).ToList();
            reportViewModel.TotalRevenue = revenue.Sum(r => r.Revenue);
            reportViewModel.RevenuePerDays = revenue;

            return View(reportViewModel);
        }

        //GET: Admin/ReportsChart
        public IActionResult ReportsChart(DateTime? fromD, DateTime? toD)
        {
            var fromDate = fromD?.Date ?? DateTime.Now.AddDays(-30).Date;
            var toDate = (toD?.Date.AddDays(1).AddSeconds(-1)) ?? DateTime.Now.Date.AddDays(1).AddSeconds(-1);


            ViewBag.FromDate = fromDate.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate.ToString("yyyy-MM-dd");

            var reportViewModel = new AdminReportViewModel();

            //User
            var newUsers = _context.Users
                .Where(u => u.CreatedTime >= fromDate && u.CreatedTime <= toDate)
                .GroupBy(u => u.CreatedTime.Date)
                .Select(g => new UserPerDay
                {
                    Date = g.Key,
                    Count = g.Count()
                }).ToList();
            reportViewModel.TotalNewUsers = newUsers.Sum(u => u.Count);
            reportViewModel.NewUsersPerDay = newUsers;


            //Erroled
            var erroledUsers = _context.UserCourses
                .Where(u => u.RegistedTime >= fromDate && u.RegistedTime <= toDate)
                .GroupBy(u => u.RegistedTime.Date)
                .Select(g => new ErroledPerDay
                {
                    Date = g.Key,
                    Count = g.Count()
                }).ToList();
            reportViewModel.TotalErroled = erroledUsers.Sum(u => u.Count);
            reportViewModel.ErroledPerDays = erroledUsers;

            //Revenue
            var revenue = _context.UserCourses
                .Where(uc => uc.RegistedTime >= fromDate && uc.RegistedTime <= toDate)
                .GroupBy(uc => uc.RegistedTime.Date)
                .Select(g => new RevenuePerDay
                {
                    Date = g.Key,
                    Revenue = g.Sum(uc => _context.Courses.FirstOrDefault(c => c.CourseId == uc.CourseId).Cost)
                }).ToList();
            reportViewModel.TotalRevenue = revenue.Sum(r => r.Revenue);
            reportViewModel.RevenuePerDays = revenue;

            return View(reportViewModel);
        }
    }
}

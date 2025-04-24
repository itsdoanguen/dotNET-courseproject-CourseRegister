using System.Security.Claims;
using dotNET_courseproject_CourseRegister.Attributes;
using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.Models;
using dotNET_courseproject_CourseRegister.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    [Authorize(Roles = "Student")]
    [AuthorizeStudent]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Student/Index
        public IActionResult Index()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                return NotFound();
            }
            var courseList = new StudentIndexViewModel
            {
                CourseList = GetCourseList(userId),
                StudentCourse = GetStudentCourseList(userId)
            };
            return View(courseList);
        }

        //GET: Student/Profile
        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            var userViewModel = new StudentProfileViewModel
            {
                UserId = user.UserId,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                DOB = user.DOB,
                CreatedTime = user.CreatedTime,
                Money = user.Money,
                Role = user.Role.ToString(),
                TotalEnrolledCourses = _context.UserCourses.Count(uc => uc.UserId == userId)
            };
            return View(userViewModel);
        }

        //GET: Student/MyCourse
        public async Task<IActionResult> MyCourse()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                return NotFound();
            }
            var courseList = GetStudentCourseList(userId);
            return View(courseList);
        }

        //GET: Student/Settings
        public IActionResult Settings()
        {

            return View();
        }

        //GET: Student/CourseList
        [HttpGet]
        public IActionResult CourseList()
        {

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                return NotFound();
            }
            var courseList = GetCourseList(userId);

            var normalCourseList = new List<CourseList>();
            var nearlyFullCourseList = new List<CourseList>();
            var fullOrClosedCourseList = new List<CourseList>();

            foreach (var course in courseList)
            {
                if (course.CurrentStudents >= course.MaxStudents || course.StartedTime < DateTime.Now)
                {
                    fullOrClosedCourseList.Add(course);
                }
                else if (course.CurrentStudents >= course.MaxStudents * 0.8 || (course.StartedTime - DateTime.Now).TotalDays <= 7)
                {
                    nearlyFullCourseList.Add(course);
                }
                else
                {
                    normalCourseList.Add(course);
                }
            }

            var courseListViewModel = new StudentCourseListViewModel
            {
                TotalCourses = courseList.Count,
                NormalCourses = normalCourseList,
                NearlyFullCourses = nearlyFullCourseList,
                FullOrClosedCourses = fullOrClosedCourseList
            };

            return View(courseListViewModel);
        }

        //GET: Student/CourseDetails
        [HttpGet]
        public IActionResult CourseDetails(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (userId == 0)
            {
                return NotFound();
            }
            var courseList = GetCourseList(userId).FirstOrDefault(c => c.CourseId == id);
            if (courseList == null)
            {
                return NotFound();
            }
            var isEnrroled = _context.UserCourses.Any(c => c.CourseId == id && c.UserId == int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            var courseViewModel = new StudentCourseDetailViewModel
            {
                CourseLists = courseList,
                IsEnrolled = isEnrroled,
            };
            return View(courseViewModel);
        }

        //POST: Student/CourseRegister
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseRegister(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            if (course.Status != Course.CourseStatus.Active)
            {
                TempData["ErrorMessage"] = "Khóa học không còn hoạt động. Bạn không thể đăng ký!";
                return RedirectToAction("CourseDetails", new { id });
            }
            if (course.CurrentStudents >= course.MaxStudents)
            {
                TempData["ErrorMessage"] = "Khóa học đã đạt số lượng học viên tối đa. Xin vui lòng chọn khóa học khác!";
                return RedirectToAction("CourseDetails", new { id });
            }
            if (course.StartedTime < DateTime.Now)
            {
                TempData["ErrorMessage"] = "Khóa học đã bắt đầu. Bạn không thể đăng ký!";
                return RedirectToAction("CourseDetails", new { id });
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userContext = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (userContext == null)
            {
                return NotFound();
            }
            if (userContext.Money < course.Cost)
            {
                TempData["ErrorMessage"] = "Số dư tài khoản không đủ để đăng ký khóa học này!";
                return RedirectToAction("CourseDetails", new { id });
            }

            var userCourse = new User_Course
            {
                CourseId = id,
                UserId = userId,
                RegistedTime = DateTime.Now
            };
            await _context.UserCourses.AddAsync(userCourse);

            course.CurrentStudents++;
            _context.Courses.Update(course);

            userContext.Money -= course.Cost;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Đăng ký khóa học thành công!";
            return RedirectToAction("CourseDetails", new { id });
        }
        //POST: Student/CourseCancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CourseCancel(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var userCourse = _context.UserCourses.FirstOrDefault(c => c.CourseId == id && c.UserId == userId);
            if (userCourse == null)
            {
                TempData["ErrorMessage"] = "Bạn chưa đăng ký khóa học này!";
                return RedirectToAction("CourseDetails", new { id });
            }

            if (course.StartedTime < DateTime.Now && course.Status == Course.CourseStatus.Active)
            {
                TempData["ErrorMessage"] = "Khóa học đã bắt đầu. Bạn không thể hủy đăng ký!";
                return RedirectToAction("CourseDetails", new { id });
            }

            var userContext = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (userContext == null)
            {
                return NotFound();
            }
            userContext.Money += course.Cost;

            _context.UserCourses.Remove(userCourse);
            course.CurrentStudents--;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Hủy đăng ký khóa học thành công!";

            if (course.Status == Course.CourseStatus.Inactive)
            {
                return RedirectToAction("CourseList"); 
            }
            return RedirectToAction("CourseDetails", new { id });
        }
        //GET: Student/Deposit
        [HttpGet]
        public IActionResult Deposit()
        {
            return View();
        }
        //POST: Student/Deposit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(double money)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            if (money <= 0)
            {
                TempData["ErrorMessage"] = "Số tiền nạp không hợp lệ!";
                return RedirectToAction("Profile");
            }
            if (user.Money + money > 1000000000)
            {
                TempData["ErrorMessage"] = "Số dư tài khoản không được vượt quá 1 tỷ đồng!";
                return RedirectToAction("Profile");
            }
            user.Money += money;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Nạp tiền thành công!";
            return RedirectToAction("Profile");
        }

        //Methods
        private List<CourseList> GetCourseList(int? userId = null)
        {
            var coursesQuery = _context.Courses.AsQueryable();

            if (userId.HasValue)
            {
                var enrolledCourseIds = _context.UserCourses
                    .Where(uc => uc.UserId == userId.Value)
                    .Select(uc => uc.CourseId)
                    .ToList();

                coursesQuery = coursesQuery.Where(c =>
                    c.Status == Course.CourseStatus.Active ||
                    (c.Status == Course.CourseStatus.Inactive && enrolledCourseIds.Contains(c.CourseId)));
            }
            else
            {
                coursesQuery = coursesQuery.Where(c => c.Status == Course.CourseStatus.Active);
            }

            var courses = coursesQuery.ToList();


            var courseList = new List<CourseList>();
            foreach (var course in courses)
            {
                var courseViewModel = new CourseList
                {
                    CourseId = course.CourseId,
                    CourseName = course.CourseName,
                    TeacherName = course.TeacherName,
                    CourseDescription = course.CourseDescription,
                    StartedTime = course.StartedTime,
                    Cost = course.Cost,
                    MaxStudents = course.MaxStudents,
                    CurrentStudents = course.CurrentStudents,
                    Duration = course.Duration,
                    CourseImage = course.CourseImage,
                    Status = course.Status.ToString()
                };
                courseList.Add(courseViewModel);
            }
            return courseList;
        }
        private List<CourseList> GetStudentCourseList(int userId)
        {
            return GetCourseList(userId)
                .Where(c => _context.UserCourses.Any(uc => uc.UserId == userId && uc.CourseId == c.CourseId))
                .ToList();
        }

    }
}

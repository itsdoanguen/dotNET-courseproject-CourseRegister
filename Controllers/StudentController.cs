using System.Security.Claims;
using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.Models;
using dotNET_courseproject_CourseRegister.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    [Authorize(Roles = "Student")]
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
            var courseList = new StudentIndexViewModel
            {
                CourseList = GetCourseList(),
                StudentCourse = GetStudentCourseList(userId)
            };
            return View(courseList);
        }
        //GET: Student/CourseList
        [HttpGet]
        public IActionResult CourseList()
        {
            var courseList = GetCourseList();
            
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
            var courseList = GetCourseList().FirstOrDefault(c => c.CourseId == id);
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
            var userCourse = new User_Course
            {
                CourseId = id,
                UserId = userId,
                RegistedTime = DateTime.Now
            };
            await _context.UserCourses.AddAsync(userCourse);

            course.CurrentStudents++;
            _context.Courses.Update(course);
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

            if (course.StartedTime < DateTime.Now)
            {
                TempData["ErrorMessage"] = "Khóa học đã bắt đầu. Bạn không thể hủy đăng ký!";
                return RedirectToAction("CourseDetails", new { id });
            }


            _context.UserCourses.Remove(userCourse);
            course.CurrentStudents--;
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Hủy đăng ký khóa học thành công!";
            return RedirectToAction("CourseDetails", new { id });
        }


        //Methods
        private List<CourseList> GetCourseList()
        {
            var courses = _context.Courses.Where(c => c.Status == Course.CourseStatus.Active).ToList();
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
                    CourseImage = course.CourseImage
                };
                courseList.Add(courseViewModel);
            }
            return courseList;
        }
        private List<CourseList> GetStudentCourseList(int id)
        {
            var courseList = new List<CourseList>();
            var coursesID = _context.UserCourses.Where(c => c.UserId == id).ToList();
            foreach (var course in coursesID)
            {
                var courseContext = _context.Courses.FirstOrDefault(c => c.CourseId == course.CourseId);
                if (courseContext != null && courseContext.Status == Course.CourseStatus.Active)
                {
                    var courseViewModel = GetCourseList().FirstOrDefault(c => c.CourseId == courseContext.CourseId);
                    courseList.Add(courseViewModel);
                }
            }
            return courseList;
        }
    }
}

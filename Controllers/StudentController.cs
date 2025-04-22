using System.Security.Claims;
using dotNET_courseproject_CourseRegister.Data;
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
            var course = _context.Courses.ToList();
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
            var courseList = _context.Courses.ToList();
            var courseListViewModel = new StudentCourseListViewModel
            {
                TotalCourses = courseList.Count,
                CourseList = GetCourseList()
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
        private List<CourseList> GetCourseList()
        {
            var courses = _context.Courses.ToList();
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
                if (courseContext != null)
                {
                    var courseViewModel = GetCourseList().FirstOrDefault(c => c.CourseId == courseContext.CourseId);
                    courseList.Add(courseViewModel);
                }
            }
            return courseList;
        }
    }
}

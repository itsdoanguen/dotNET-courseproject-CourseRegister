using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.Models;
using dotNET_courseproject_CourseRegister.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //GET: Course/Create
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateCourse()
        {
            return View();
        }
        //POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCourse(CourseCreateViewModel course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            var existingCourse = _context.Courses.FirstOrDefault(c => c.CourseName == course.CourseName);

            if (existingCourse != null)
            {
                ModelState.AddModelError("CourseName", "Đã tồn tại một khóa học tên như thế này");
                return View(course);
            }

            var newCourse = new Course
            {
                CourseName = course.CourseName,
                TeacherName = course.TeacherName,
                StartedTime = course.StartedTime,
                Cost = course.Cost,
                MaxStudents = course.MaxStudents,
                Duration = course.Duration,
                CourseDescription = course.CourseDescription
            };

            await _context.Courses.AddAsync(newCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCourses","Admin");
        }
    }
}

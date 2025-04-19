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
        public async Task<IActionResult> CreateCourse(CourseEditorViewModel course)
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
                CourseDescription = course.CourseDescription,
                Status = Course.CourseStatus.Active
            };

            await _context.Courses.AddAsync(newCourse);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCourses", "Admin");
        }
        //GET: Course/EditCourse
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult EditCourse(int id)
        {
            var course = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            var courseViewModel = new CourseEditorViewModel
            {
                CourseName = course.CourseName,
                TeacherName = course.TeacherName,
                StartedTime = course.StartedTime,
                Cost = course.Cost,
                MaxStudents = course.MaxStudents,
                Duration = course.Duration,
                CourseDescription = course.CourseDescription
            };
            return View(courseViewModel);
        }
        //POST: Course/EditCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditCourse(int id, CourseEditorViewModel course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            var existingCourse = _context.Courses.FirstOrDefault(c => c.CourseId == id);
            if (existingCourse == null)
            {
                return NotFound();
            }

            existingCourse.CourseName = course.CourseName;
            existingCourse.TeacherName = course.TeacherName;
            existingCourse.StartedTime = course.StartedTime;
            existingCourse.Cost = course.Cost;
            existingCourse.MaxStudents = course.MaxStudents;
            existingCourse.Duration = course.Duration;
            existingCourse.CourseDescription = course.CourseDescription;

            _context.Courses.Update(existingCourse);
            await _context.SaveChangesAsync();
            
            return RedirectToAction("ManageCourses", "Admin");
        }
        //POST: Course/DeleteCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);
            
            if (course == null)
            {
                return NotFound();
            }

            course.Status = Course.CourseStatus.Inactive;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("ManageCourses", "Admin");
        }
        // POST: Course/RestoreCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            course.Status = Course.CourseStatus.Active;

            _context.Courses.Update(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCourses", "Admin");
        }

        // POST: Course/PerDeleteCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PerDeleteCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction("ManageCourses", "Admin");
        }
    }
}
using System.Threading.Tasks;
using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.Models;
using dotNET_courseproject_CourseRegister.ViewModels.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    public class CourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET: Course/Details
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CourseDetails(int id)
        {
            var course = await _context.Courses
                .FirstOrDefaultAsync(c => c.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            var enrroledUser = await _context.UserCourses.Where(c => c.CourseId == id).Include(uc => uc.User).Select(uc => new EnrolledUserViewModel
            {
                UserId = uc.UserId,
                UserName = uc.User.UserName,
                Email = uc.User.Email,
                EnrolledDate = uc.RegistedTime
            }).ToListAsync();

            var courseViewModel = new CourseDetailsViewModel
            {
                CourseName = course.CourseName,
                TeacherName = course.TeacherName,
                CourseDescription = course.CourseDescription,
                StartedTime = course.StartedTime,
                Cost = course.Cost,
                MaxStudents = course.MaxStudents,
                CurrentStudents = course.CurrentStudents,
                Duration = course.Duration,
                CourseImage = course.CourseImage,
                EnrolledUsers = enrroledUser
            };
            return View(courseViewModel);
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

            string? imagePath = null;
            if (course.CourseImage != null && course.CourseImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(course.CourseImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await course.CourseImage.CopyToAsync(stream);
                }

                imagePath = "/images/" + uniqueFileName;
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
                Status = Course.CourseStatus.Active,
                CourseImage = imagePath ?? "/images/default.jpg"
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

            if (course.CourseImage != null && course.CourseImage.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(course.CourseImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await course.CourseImage.CopyToAsync(stream);
                }

                existingCourse.CourseImage = "/images/" + uniqueFileName;
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
        [Authorize(Roles = "Admin")]
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
        //POST: Course/PerDeleteCourse
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PerDeleteCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.EnrolledUsers)
                .FirstOrDefaultAsync(c => c.CourseId == id);

            if (course == null)
            {
                return NotFound();
            }

            if (course.EnrolledUsers != null && course.EnrolledUsers.Count > 0)
            {
                TempData["ErrorMessage"] = "Không thể xóa khóa học này vì có học viên đã đăng ký.";
                return RedirectToAction("ManageCourses", "Admin");
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Xóa khóa học thành công.";
            return RedirectToAction("ManageCourses", "Admin");
        }

    }
}
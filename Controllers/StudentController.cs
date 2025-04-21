using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.ViewModels.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    [Authorize(Roles ="Student")]
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET: Student/CourseList
        [HttpGet]
        public IActionResult CourseList()
        {
            var courseList = _context.Courses.ToList();
            var courseListViewModel = new StudentCourseListViewModel
            {
                TotalCourses = courseList.Count,
                CourseList = courseList.Select(c => new CourseList
                {
                    CourseName = c.CourseName,
                    TeacherName = c.TeacherName,
                    CourseDescription = c.CourseDescription,
                    StartedTime = c.StartedTime,
                    Cost = c.Cost,
                    MaxStudents = c.MaxStudents,
                    CurrentStudents = c.CurrentStudents,
                    Duration = c.Duration,
                    CourseImage = c.CourseImage
                }).ToList()
            };
            return View(courseListViewModel);
        }
    }
}

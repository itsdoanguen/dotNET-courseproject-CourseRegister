using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    [Authorize]
    public class CourseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

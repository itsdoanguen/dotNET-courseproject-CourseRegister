using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

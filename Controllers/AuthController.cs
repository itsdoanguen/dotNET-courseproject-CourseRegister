using System.Diagnostics;
using System.Security.Claims;
using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.Models;
using dotNET_courseproject_CourseRegister.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    public class AuthController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        //GET: Auth/Index
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }
        //GET: Auth/AccessDenied
        public IActionResult AccessDenied()
        {
            return PartialView("_AccessDeniedPartial");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //GET: Auth/Error
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //GET: Auth/Register
        public IActionResult Register()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }
        //POST: Auth/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var usedEmail = _context.Users.FirstOrDefault(s => s.Email == model.Email);
            if (usedEmail != null)
            {
                ModelState.AddModelError("Email", "Email này đã được sử dụng");
                return View(model);
            }
            var usedUsername = _context.Users.FirstOrDefault(s => s.UserName == model.UserName);
            if (usedUsername != null)
            {
                ModelState.AddModelError("UserName", "Tên tài khoản này đã được sử dụng");
                return View(model);
            }

            var user = new User
            {
                UserName = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Email = model.Email,
                Role = Models.User.UserRole.Student
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            await SignIn(user);
            return RedirectToAction("Index", "Student");
        }
        //GET: Auth/Login
        public IActionResult Login()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (User.IsInRole("Student"))
                {
                    return RedirectToAction("Index", "Student");
                }
            }
            return View();
        }
        //POST: Auth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _context.Users.FirstOrDefault(s => s.UserName == model.UserNameOrEmail || s.Email == model.UserNameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("UserNameOrEmail", "Tên tài khoản hoặc email không tồn tại");
                return View(model);
            }
            if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                ModelState.AddModelError("Password", "Mật khẩu không đúng");
                return View(model);
            }
            await SignIn(user);

            return RedirectToAction("Index", "Auth");
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Auth");
        }

        public async Task SignIn(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }
    }
}

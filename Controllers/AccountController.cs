using System.Security.Claims;
using dotNET_courseproject_CourseRegister.Data;
using dotNET_courseproject_CourseRegister.ViewModels.Student;
using Microsoft.AspNetCore.Mvc;

namespace dotNET_courseproject_CourseRegister.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }
        //GET: Account/EditProfile
        [HttpGet]
        public IActionResult EditProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            var userVM = new StudentEditorViewModel
            {
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                DOB = user.DOB
            };
            return View(userVM);
        }
        //POST: Account/EditProfile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(StudentEditorViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;
            user.PhoneNumber = model.PhoneNumber;
            user.DOB = model.DOB;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction("Profile","Student");
        }
        //GET: Account/EditAccount
        [HttpGet]
        public IActionResult EditAccount()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            var userVM = new StudentEditorViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };
            return View(userVM);
        }
        //POST: Account/EditAccount
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(StudentEditorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }

            bool isUserNameChanged = !string.Equals(user.UserName, model.UserName, StringComparison.Ordinal);
            bool isEmailChanged = !string.Equals(user.Email, model.Email, StringComparison.Ordinal);

            if (isUserNameChanged || isEmailChanged)
            {
                if (string.IsNullOrWhiteSpace(model.Password))
                {
                    ModelState.AddModelError("Password", "Bạn cần nhập mật khẩu để xác nhận thay đổi.");
                    return View(model);
                }

                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    ModelState.AddModelError("Password", "Mật khẩu không đúng.");
                    return View(model);
                }
            }

            // Cập nhật thông tin
            user.UserName = model.UserName;
            user.Email = model.Email;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật tài khoản thành công!";
            return RedirectToAction("Profile", "Student");
        }

        //GET: Account/ChangePassword
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }
            var userVM = new StudentEditorViewModel
            {
                UserName = user.UserName,
                Email = user.Email
            };
            return View(userVM);
        }
        //POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(StudentEditorViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (user == null)
            {
                return NotFound();
            }

            if (!BCrypt.Net.BCrypt.Verify(model.OldPassword, user.Password))
            {
                ModelState.AddModelError("OldPassword", "Mật khẩu cũ không đúng.");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Mật khẩu xác nhận không khớp.");
                return View(model);
            }
            user.Password = model.NewPassword;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Cập nhật mật khẩu thành công!";
            return RedirectToAction("Profile", "Student");
        }
    }
}

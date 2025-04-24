using dotNET_courseproject_CourseRegister.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotNET_courseproject_CourseRegister.Attributes
{
    public class AuthorizeStudentAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = (User)context.HttpContext.Items["User"];
            if (user != null && !user.IsActive)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Auth", null);
            }
        }
    }
}

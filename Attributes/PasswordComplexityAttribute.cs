using System.ComponentModel.DataAnnotations;

namespace dotNET_courseproject_CourseRegister.Attributes
{
    public class PasswordComplexityAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Yêu cầu phải có mật khẩu");
            }

            var password = value.ToString();
        
            if (password.Length < 6 || password.Length > 18)
            {
                return new ValidationResult("Độ dài mật khẩu tối thiểu là 6 và tối đa là 18");
            }
            if (!password.Any(char.IsDigit))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất một số");
            }
            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                return new ValidationResult("Mật khẩu phải chứa ít nhất một kí hiệu đặc biệt");
            }
            if (password.Contains(" "))
            {
                return new ValidationResult("Mật khẩu không được chứa khoảng trắng!");
            }

            return ValidationResult.Success;
        }
    }
}

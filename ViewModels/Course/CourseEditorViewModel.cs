using System.ComponentModel.DataAnnotations;

namespace dotNET_courseproject_CourseRegister.ViewModels.Course
{
    public class CourseEditorViewModel
    {

        [Required(ErrorMessage = "Không được để trống tên khóa học")]
        [Display(Name = "Tên khóa học")]
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Không được để trống tên giáo viên")]
        [Display(Name = "Tên giáo viên")]
        public string TeacherName { get; set; }
        [Required(ErrorMessage = "Không được để trống mô tả khóa học")]
        [Display(Name = "Mô tả khóa học")]
        public string CourseDescription { get; set; }
        [Required(ErrorMessage = "Không được để trống thời gian bắt đầu")]
        [DataType(DataType.Date)]
        [Display(Name = "Thời gian bắt đầu")]
        public DateTime StartedTime { get; set; }
        [Required(ErrorMessage = "Không được để trống học phí")]
        [Range(0, int.MaxValue, ErrorMessage = "Học phí không hợp lệ")]
        [Display(Name = "Học phí")]
        public double Cost { get; set; }
        [Required(ErrorMessage = "Không được để trống số lượng học viên tối đa")]
        [Range(0, 60, ErrorMessage = "Số lượng học viên tối đa không hợp lệ")]
        [Display(Name = "Số lượng học viên tối đa")]
        public int MaxStudents { get; set; }
        [Required]
        [Display(Name = "Thời lượng khóa học (Buổi)")]
        public int Duration { get; set; }
        public IFormFile? CourseImage { get; set; }

    }
}

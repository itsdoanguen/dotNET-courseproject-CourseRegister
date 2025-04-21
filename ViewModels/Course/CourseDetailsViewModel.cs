namespace dotNET_courseproject_CourseRegister.ViewModels.Course
{
    public class CourseDetailsViewModel
    {
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string CourseDescription { get; set; }
        public DateTime StartedTime { get; set; }
        public decimal Cost { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public int Duration { get; set; }
        public string CourseImage { get; set; } = "default.jpg";

        public List<EnrolledUserViewModel> EnrolledUsers { get; set; } = new List<EnrolledUserViewModel>();
    }

    public class EnrolledUserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime EnrolledDate { get; set; }
    }
}

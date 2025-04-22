namespace dotNET_courseproject_CourseRegister.ViewModels.Student
{
    public class StudentCourseDetailViewModel
    {
        public bool IsEnrolled { get; set; } = false;
        public CourseList CourseLists { get; set; } = new CourseList();
    }
}

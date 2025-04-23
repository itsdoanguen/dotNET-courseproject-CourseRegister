namespace dotNET_courseproject_CourseRegister.ViewModels.Admin
{
    public class AdminManageCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public DateTime StartedTime { get; set; }
        public double Cost { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public int Duration { get; set; }
        public CourseStatus Status { get; set; }

        public enum CourseStatus
        {
            Active,
            Inactive
        }
    }
}

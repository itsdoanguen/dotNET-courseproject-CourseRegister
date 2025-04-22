namespace dotNET_courseproject_CourseRegister.ViewModels.Student
{
    public class StudentCourseListViewModel
    {
        public int TotalCourses { get; set; } = 0;

        public List<CourseList> NormalCourses { get; set; } = new List<CourseList>();
        public List<CourseList> NearlyFullCourses { get; set; } = new List<CourseList>();
        public List<CourseList> FullOrClosedCourses { get; set; } = new List<CourseList>();
    }

    public class CourseList
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string TeacherName { get; set; }
        public string CourseDescription { get; set; }
        public DateTime StartedTime { get; set; }
        public int Cost { get; set; }
        public int MaxStudents { get; set; }
        public int CurrentStudents { get; set; }
        public int Duration { get; set; }
        public string CourseImage { get; set; } = "default.jpg";
    }
}

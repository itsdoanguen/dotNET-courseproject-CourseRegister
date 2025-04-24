namespace dotNET_courseproject_CourseRegister.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }

        public int TotalCourses { get; set; }
        public int TotalActiveCourses { get; set; }
        public int TotalInactiveCourses { get; set; }
        public double ActiveCourseRatio => TotalCourses == 0 ? 0 : (double)TotalActiveCourses / TotalCourses * 100;
        public double InactiveCourseRatio => 100 - ActiveCourseRatio;


        public int TotalEnrolledUsers { get; set; }
        public double TotalRevenue { get; set; } = 0;


        public List<EnrolledUserViewModel> MostEnrollingUser { get; set; }
        public List<CourseDashboardViewModel> MostEnrolledCourses { get; set; }

        public List<CourseDashboardViewModel> AllCourseRevenue { get; set; } = new List<CourseDashboardViewModel>();
    }

    public class EnrolledUserViewModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public int TotalCourse { get; set; }
    }
    public class CourseDashboardViewModel
    {
        public string CourseName { get; set; }
        public int TotalEnrolledUsers { get; set; }
        public double? CourseRevenue { get; set; }
    }
}

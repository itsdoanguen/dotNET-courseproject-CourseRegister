namespace dotNET_courseproject_CourseRegister.ViewModels.Admin
{
    public class AdminReportViewModel
    {
        //User
        public int TotalNewUsers { get; set; }
        public List<UserPerDay> NewUsersPerDay { get; set; } = new List<UserPerDay>();

        //Erroled
        public int TotalErroled { get; set; }
        public List<ErroledPerDay> ErroledPerDays { get; set; } = new List<ErroledPerDay>();

        //Revenue
        public double TotalRevenue { get; set; } = 0;
        public List<RevenuePerDay> RevenuePerDays { get; set; } = new List<RevenuePerDay>();
    }
    public class UserPerDay
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
    public class ErroledPerDay
    {
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }
    public class RevenuePerDay
    {
        public DateTime Date { get; set; }
        public double Revenue { get; set; }
    }
}

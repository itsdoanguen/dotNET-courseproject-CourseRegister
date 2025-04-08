using Microsoft.EntityFrameworkCore;

namespace dotNET_courseproject_CourseRegister.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Models.User> Users { get; set; }
        public DbSet<Models.Course> Courses { get; set; }
        public DbSet<Models.User_Course> UserCourses { get; set; }
    }
}

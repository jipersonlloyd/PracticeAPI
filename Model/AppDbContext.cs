using Microsoft.EntityFrameworkCore;

namespace TestAPI.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {

        }

        public DbSet<Students> students { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Students>().HasData(
                new Students 
                {
                    studentID = 1,
                    firstName = "Lloyd Jiperson",
                    lastName = "Diaz",
                    email = "lloyddiaz0205@gmail.com",
                    gender = Gender.Male,
                    departmentID = 1
                }
               );

            modelBuilder.Entity<Students>().HasData(
                new Students
                {
                    studentID = 2,
                    firstName = "Kazami",
                    lastName = "Nazukai",
                    email = "thyronrequez2@gmail.com",
                    gender = Gender.Male,
                    departmentID = 2
                }
               );
        }
    }
}

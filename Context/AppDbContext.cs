using _5PJS.Entities;
using Microsoft.EntityFrameworkCore;

namespace _5PJS.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Doctor> doctors { get; set; }
        public DbSet<Address> addresses { get; set; }
        public DbSet<Administrator> administrators { get; set; }
        public DbSet<Diagnosis> diagnosiss { get; set; }
        public DbSet<FeedBack> feedBacks { get; set; }
        public DbSet<MethodDiagnosis> methodDiagnoses { get; set; }
        public DbSet<Patient> patients { get; set; }
        public DbSet<Report> reports { get; set; }
        public DbSet<ReportDiagnosis> reportDiagnoses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PJS;Trusted_Connection=true");
                base.OnConfiguring(optionsBuilder);
            }

        }
    }
}

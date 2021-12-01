using Microsoft.EntityFrameworkCore;

namespace ProjectBank.Server
{
    public class ProjectBankContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<View> Views { get; set; }
        public DbSet<Applicant> Applicants { get; set; }
        public ProjectBankContext(DbContextOptions<ProjectBankContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Applicant>()
                    .HasKey(a => new { a.ProjectId, a.StudentId });
            modelBuilder.Entity<View>()
                    .HasKey(a => new { a.ProjectId, a.StudentId });
        }
    }
}
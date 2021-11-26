using Microsoft.EntityFrameworkCore;

    public class ProjectBankContext: DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Supervisor> Supervisors { get; set; }
        public ProjectBankContext(DbContextOptions<ProjectBankContext>options):base(options) {}
    }
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace ProjectBank.Server
{
    public class ProjectBankContextFactory : IDesignTimeDbContextFactory<ProjectBankContext>
    {
        public ProjectBankContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("elated_babbage");

            var optionsBuilder = new DbContextOptionsBuilder<ProjectBankContext>()
                .UseSqlServer(connectionString);

            return new ProjectBankContext(optionsBuilder.Options);
        }
        public static async void Seed(ProjectBankContext context)
        {
            var facade = new DBFacade(context);
            await facade.CreateProject("mAchine", "something about AI", 2);
            var applicant = new ApplicantRepository(context);
            await applicant.ApplyToProjectAsync(1, 2);
            var view = new ViewRepository(context);
            await view.AddViewAsync(1, 2);
            context.SaveChanges();
        }
    }
}
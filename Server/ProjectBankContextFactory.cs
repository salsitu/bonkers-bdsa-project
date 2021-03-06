using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectBank.Server.Entities
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

            var connectionString = configuration.GetConnectionString("sql_server");

            var optionsBuilder = new DbContextOptionsBuilder<ProjectBankContext>()
                .UseSqlServer(connectionString);

            return new ProjectBankContext(optionsBuilder.Options);
        }
    }
}
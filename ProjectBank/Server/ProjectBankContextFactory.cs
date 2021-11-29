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

            var connectionString = configuration.GetConnectionString("brave_archimedes");

            var optionsBuilder = new DbContextOptionsBuilder<ProjectBankContext>()
                .UseSqlServer(connectionString);

            return new ProjectBankContext(optionsBuilder.Options);
        }
    }
}
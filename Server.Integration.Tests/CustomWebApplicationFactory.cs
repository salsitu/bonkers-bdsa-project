using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProjectBank.Server.Entities;
using System;
using System.Linq;

namespace Server.Integration.Tests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ProjectBankContext>));

            if (dbContext != null)
            {
                services.Remove(dbContext);
            }

            /* Overriding policies and adding Test Scheme defined in TestAuthHandler */
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes("Test")
                    .Build();

                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
                options.DefaultScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

            var connection = new SqliteConnection("Filename=:memory:");

            services.AddDbContext<ProjectBankContext>(options =>
            {
                options.UseSqlite(connection);
            });

            var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            using var appContext = scope.ServiceProvider.GetRequiredService<ProjectBankContext>();
            appContext.Database.OpenConnection();
            appContext.Database.EnsureCreated();

            Seed(appContext);
        });

        builder.UseEnvironment("Integration");

        return base.CreateHost(builder);
    }

    private void Seed(ProjectBankContext context) 
    {
        context.Projects.Add(new Project
        {
            Id = 1,
            Name = "huhu",
            Description = "jaja",
            AuthorId = 1
        });
        context.Projects.Add(new Project
        {
            Id = 2,
            Name = "hihi",
            Description = "dada",
            AuthorId = 2
        });
        context.Users.Add(new User
        {
            Id = 1,
            Name = "paolo",
            IsSupervisor = true,
            Email = "paolo@"
        });
        context.Users.Add(new User
        {
            Id = 2,
            Name = "jakob",
            IsSupervisor = false,
            Email = "jakob@"
        });
        context.Users.Add(new User
        {
            Id = 3,
            Name = "Barto",
            IsSupervisor = false,
            Email = "barto@"
        });
        context.Users.Add(new User
        {
            Id = 4,
            Name = "Dennis",
            IsSupervisor = false,
            Email = "Dennis@"
        });
        context.Applicants.Add(new Applicant
        {
            ProjectId = 1,
            StudentId = 2,
        });
        context.Applicants.Add(new Applicant
        {
            ProjectId = 2,
            StudentId = 2,
        });
        context.Applicants.Add(new Applicant
        {
            ProjectId = 1,
            StudentId = 3,
        });
        context.Views.Add(new View
        {
            ProjectId = 1,
            StudentId = 2
        });
        context.Views.Add(new View
        {
            ProjectId = 1,
            StudentId = 5
        });
        context.SaveChanges(); 
    }



}


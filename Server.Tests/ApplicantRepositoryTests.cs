using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Server;
using ProjectBank.Server.Entities;
using static ProjectBank.Server.Entities.Response;
using Xunit;

namespace Server.Tests;

public class ApplicantRepositoryTests
{
    private readonly ProjectBankContext _context;
    private readonly ApplicantRepository _repo;

    public ApplicantRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();
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
            Name = "troels",
            IsSupervisor = false,
            Email = "troels@"
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
        context.SaveChanges();
        _context = context;
        _repo = new ApplicantRepository(_context);
    }

    [Fact]
    public async Task ApplyToProjectAsync_returns_created_when_new_application()
    {
        var application = await _repo.ApplyToProjectAsync(2, 3);

        Assert.Equal(Created, application);
    }
    [Fact]
    public async Task ApplyToProjectAsync_returns_conflict_if_relation_already_exists()
    {
        var application = await _repo.ApplyToProjectAsync(2, 2);

        Assert.Equal(Conflict, application);
    }
    [Fact]
    public async Task HasAlreadyAppliedToProjectAsync_returns_notFound_if_relation_does_not_exists()
    {
        var application = await _repo.HasAlreadyAppliedToProjectAsync(2, 4);

        Assert.Equal(NotFound, application);
    }
    [Fact]
    public async Task HasAlreadyAppliedToProjectAsync_returns_exists_if_relation_is_found()
    {
        var application = await _repo.HasAlreadyAppliedToProjectAsync(1, 2);

        Assert.Equal(Exists, application);
    }
    [Fact]
    public async Task ShowListOfAppliedProjectsAsync_returns_list_of_applied_projects()
    {
        var application = await _repo.ShowListOfAppliedProjectsAsync(2);

        Assert.Equal(new List<SimplifiedProjectDTO> {new SimplifiedProjectDTO(1, "huhu"), new SimplifiedProjectDTO(2, "hihi") }, application);
    }
    [Fact]
    public async Task SelectNrOfProjectApplicationsAsync_returns_2_if_project_has_two_applications()
    {
        var application = await _repo.SelectNrOfProjectApplicationsAsync(1);

        Assert.Equal(2, application);
    }
    [Fact]
    public async Task SelectNrOfProjectApplicationsAsync_returns_null_if_no_applications_to_project_exist()
    {
        var application = await _repo.SelectNrOfProjectApplicationsAsync(3);

        Assert.Equal(0, application);
    }
    [Fact]
    public async Task DeleteApplicationAsync_returns_deleted_if_project_applications_have_been_deleted()
    {
        var application = await _repo.DeleteApplicationsAsync(1);

        Assert.Equal(Deleted, application);
    }
    [Fact]
    public async Task DeleteApplicationAsync_returns_notFound_if_project_has_no_applications()
    {
        var application = await _repo.DeleteApplicationsAsync(3);

        Assert.Equal(NotFound, application);
    }
}

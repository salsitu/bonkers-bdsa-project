using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using ProjectBank.Server;
using static ProjectBank.Server.Response;
using Xunit;

namespace Server.Tests;

public class ProjectRepositoryTests
{
    private readonly ProjectBankContext _context;
    private readonly ProjectRepository _repo;

    public ProjectRepositoryTests() 
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();
        context.Projects.Add(new Project{ Id = 1,
                                            Name = "huhu",
                                            Desc = "jaja",
                                            AuthorId = 1,
                                            NrOfViews = 12,
                                            Applicants = new HashSet<int>{ 1, 2 }});
        context.Projects.Add(new Project { Id = 2, 
                                            Name = "hihi",
                                            Desc = "dada",
                                            AuthorId = 2,
                                            NrOfViews = 10,
                                            Applicants = new HashSet<int>{ 1, 2 }});
        context.Supervisors.Add(new Supervisor{ Id = 1,
                                                Name = "paolo"});
        context.Students.Add(new Student {Id = 1,
                                            Name = "Jakob"});
        context.SaveChanges();
        _context = context;
        _repo = new ProjectRepository(_context);
    }

    [Fact]
    public async Task CreateAsync_given_Project_returns_Created_with_Project()
    {
        var project = new ProjectCreateDTO("AI", "stuff about AI", 1);

        var created = await _repo.CreateAsync(project);

        Assert.Equal((Created, new ProjectDTO(3, "AI", "stuff about AI", 1)), created);
    }
    [Fact]
    public async Task CreateAsync_given_existing_Project_returns_Conflict_with_Project()
    {
        var project = new ProjectCreateDTO("hihi", "dada", 2);

        var created = await _repo.CreateAsync(project);

        Assert.Equal((Conflict, new ProjectDTO(2, "hihi", "dada", 2)), created);
    }
    [Fact]
    public async Task ReadAsync_given_non_existing_id_returns_null()
    {
        var project = await _repo.ReadAsync(36);

        Assert.Null(project);
    }
    [Fact]
    public async Task ReadAsync_given_existing_id_returns_project()
    {
        var project = await _repo.ReadAsync(2);

        Assert.Equal(new ProjectDTO(2, "hihi", "dada", 2), project);
    }
    [Fact]
    public async Task DeleteAsync_given_non_existing_id_returns_notFound()
    {
        var response = await _repo.DeleteAsync(36);

        Assert.Equal(NotFound, response);
    }
    [Fact]
    public async Task DeleteAsync_Deletes_existing_project_and_returns_Deleted()
    {
        var response = await _repo.DeleteAsync(2);

        Assert.Equal(Deleted, response);
    }
    [Fact]
    public async Task UpdateViewsAsync_updates_NrOfViews_returns_response()
    {
        var project = new ProjectDTO(2, "hihi", "dada", 2);
    }
}
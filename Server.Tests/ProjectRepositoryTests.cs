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
                                            Name = "Title1",
                                            Description = "Desc1",
                                            AuthorId = 1});
        context.Projects.Add(new Project { Id = 2, 
                                            Name = "Title2",
                                            Description = "Desc2",
                                            AuthorId = 2});
        context.Users.Add(new User{ Id = 1,
                                    Name = "paolo",
                                    IsSupervisor = true,
                                    Email = "paolo@"});
        context.Users.Add(new User{ Id = 2,
                                    Name = "jakob",
                                    IsSupervisor = false,
                                    Email = "jakob@"});
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
        var project = new ProjectCreateDTO("Title2", "Desc2", 2);

        var created = await _repo.CreateAsync(project);

        Assert.Equal((Conflict, new ProjectDTO(2, "Title2", "Desc2", 2)), created);
    }
    [Fact]
    public async Task ReadAsync_given_non_existing_id_returns_null()
    {
        var project = await _repo.GetProjectAsync(36);

        Assert.Null(project);
    }
    [Fact]
    public async Task ReadAsync_given_existing_id_returns_project()
    {
        var project = await _repo.GetProjectAsync(2);

        Assert.Equal(new ProjectDTO(2, "Title2", "Desc2", 2), project);
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
    public async Task ListAllProjectsAsync_returns_all_existing_projects()
    {
        var projectList = await _repo.GetAllProjectsAsync();


        Assert.Equal(new List<SimplifiedProjectDTO>{ new SimplifiedProjectDTO(1, "Title1"), new SimplifiedProjectDTO(2, "Title2")}, projectList);
    }
    [Fact]
    public async Task ListAllProjectsAsync_with_no_projects_returns_empty_list()
    {
        await _repo.DeleteAsync(1);
        await _repo.DeleteAsync(2);
        var projectList = await _repo.GetAllProjectsAsync();
        var emptyList = new List<SimplifiedProjectDTO>();
        Assert.Equal(emptyList, projectList);
    }
    [Fact]
    public async Task ShowCreatedProjectsAsync_returns_authors_projects()
    {
        var createdProjects = await _repo.GetCreatedProjectsAsync(1);

        Assert.Equal(new List<SimplifiedProjectDTO> { new SimplifiedProjectDTO(1, "Title1") }, createdProjects);
    }
    
}
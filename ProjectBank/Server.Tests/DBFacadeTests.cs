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

public class DBFacadeTests
{
    private readonly ProjectBankContext _context;
    private readonly DBFacade _repo;

    public  DBFacadeTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ProjectBankContext>();
        builder.UseSqlite(connection);
        var context = new ProjectBankContext(builder.Options);
        context.Database.EnsureCreated();
        context.Projects.Add(new Project{ Id = 1,
                                            Name = "huhu",
                                            Description = "jaja",
                                            AuthorId = 1});
        context.Projects.Add(new Project { Id = 2, 
                                            Name = "hihi",
                                            Description = "dada",
                                            AuthorId = 2});
        context.Users.Add(new User{ Id = 1,
                                    Name = "paolo",
                                    IsSupervisor = true});
        context.Users.Add(new User{ Id = 2,
                                    Name = "jakob",
                                    IsSupervisor = false});
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
        _repo = new DBFacade(_context);

    }

    [Fact]

    public async Task CreateProject_returns_created_and_project_if_unique_name()
    {
        var created = await _repo.CreateProject("AI", "stuff about AI", 1);

        var expected = new ProjectDTO(3,"AI", "stuff about AI", 1);

        Assert.Equal((Created, expected), created);
    }
    [Fact]
    public async Task CreateProject_returns_conflict_if_project_name_exists()
    {
        var created = await _repo.CreateProject("hihi", "this is body", 1);

        var expected = new ProjectDTO(2, "hihi", "dada", 2);

        Assert.Equal((Conflict,expected),created);
    }
    [Fact]
    public async Task SelectProject_returns_requested_project()
    {
        var project = await _repo.SelectProject(1);

        var expectedProject = new ProjectDTO(1, "huhu", "jaja",1);

        Assert.Equal(expectedProject,project);
    }
    [Fact]
    public async Task SelectProject_returns_null_if_project_doesnt_exist()
    {
        var project = await _repo.SelectProject(3);

        Assert.Null(project);
    }
    [Fact]
    public async Task DeleteProject_returns_deleted_if_project_existed()
    {
        var response = await _repo.DeleteProject(1);


        Assert.Equal(Deleted,response);       
    }

    [Fact]
    public async Task DeleteProject_returns_notfound_if_project_does_not_exist()
    {
        var response = await _repo.DeleteProject(3);
        

        Assert.Equal(NotFound,response);
    }

    [Fact]

    public async Task ShowAllProjects_returns_list_of_all_existing_projects()
    {
        var projects = await _repo.ShowAllProjects();

        var allProjects = new List<SimplifiedProjectDTO>{ new SimplifiedProjectDTO(1, "huhu"), new SimplifiedProjectDTO(2, "hihi")};

        Assert.Equal(allProjects,projects);
    }
    
    [Fact]
    public async Task ShowAllProjects_returns_empty_list_if_no_projects_exist()
    {

        await _repo.DeleteProject(1);
        await _repo.DeleteProject(2);
    
        var projects = await _repo.ShowAllProjects();

        var emptyRepo = new List<SimplifiedProjectDTO>();

        Assert.Equal(emptyRepo,projects);
    }

    [Fact]

    public async Task ShowCreatedProjects_returns_the_authors_projects()
    {
        var projects = await _repo.ShowCreatedProjects(1);

        var listOfCreatedProjects = new List<SimplifiedProjectDTO> { new SimplifiedProjectDTO(1, "huhu") };

        Assert.Equal(listOfCreatedProjects,projects);
    }

    [Fact]
    public async Task ShowCreatedProjecs_returns_empty_list_if_author_hasnt_created_projects()
    {
        await _repo.DeleteProject(1);
        var projects = await _repo.ShowCreatedProjects(1);

        var emptyListOfDTO = new List<SimplifiedProjectDTO>();

        Assert.Equal(emptyListOfDTO,projects);
    }

    
    [Fact]
    public async Task ApplyToProject_returns_created_if_application_is_new()
    {
        var newProject = _repo.CreateProject("Machine Learning", "Body", 1);

        var application = await _repo.ApplyToProject(newProject.Result.Item2.Id,2);

        Assert.Equal(Created, application);

    }
    




}

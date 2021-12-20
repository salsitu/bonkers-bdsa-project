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
        context.Users.Add(new User{ Id = 3,
                                    Name = "Barto",
                                    IsSupervisor = false,
                                    Email = "barto@"});
        context.Users.Add(new User{ Id = 4,
                                    Name = "Dennis",
                                    IsSupervisor = false,
                                    Email = "Dennis@"});
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
        var created = await _repo.CreateProject("Title2", "this is body", 1);

        var expected = new ProjectDTO(2, "Title2", "Desc2", 2);

        Assert.Equal((Conflict,expected),created);
    }
    [Fact]
    public async Task SelectProject_returns_requested_project()
    {
        var project = await _repo.GetProject(1);

        var expectedProject = new ProjectDTO(1, "Title1", "Desc1",1);

        Assert.Equal(expectedProject,project);
    }
    [Fact]
    public async Task SelectProject_returns_null_if_project_doesnt_exist()
    {
        var project = await _repo.GetProject(3);

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
        var projects = await _repo.GetAllProjects();

        var allProjects = new List<SimplifiedProjectDTO>{ new SimplifiedProjectDTO(1, "Title1"), new SimplifiedProjectDTO(2, "Title2")};

        Assert.Equal(allProjects,projects);
    }
    
    [Fact]
    public async Task ShowAllProjects_returns_empty_list_if_no_projects_exist()
    {

        await _repo.DeleteProject(1);
        await _repo.DeleteProject(2);
    
        var projects = await _repo.GetAllProjects();

        var emptyRepo = new List<SimplifiedProjectDTO>();

        Assert.Equal(emptyRepo,projects);
    }

    [Fact]

    public async Task ShowCreatedProjects_returns_the_authors_projects()
    {
        var projects = await _repo.GetCreatedProjects(1);

        var listOfCreatedProjects = new List<SimplifiedProjectDTO> { new SimplifiedProjectDTO(1, "Title1") };

        Assert.Equal(listOfCreatedProjects,projects);
    }

    [Fact]
    public async Task ShowCreatedProjecs_returns_empty_list_if_author_hasnt_created_projects()
    {
        await _repo.DeleteProject(1);
        var projects = await _repo.GetCreatedProjects(1);

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


    [Fact]

    public async Task ApplyToProject_returns_conflict_if_application_already_exists()
    {
        var application = await _repo.ApplyToProject(1,2);

        Assert.Equal(Conflict,application);
    }

    [Fact]

    public async Task HasAlreadyAppliedToProject_returns_Exists_if_application_exists()
    {
        var application = await _repo.HasAlreadyAppliedToProject(1,2);

        Assert.Equal(Exists,application);
    }


    [Fact]
    public async Task HasAlreadyAppliedToProject_returns_NotFound_if_student_has_not_applied_yet()
    {
        var application = await _repo.HasAlreadyAppliedToProject(2,3);

        Assert.Equal(NotFound,application);
    }

    [Fact]
    public async Task ShowListOfApppliedProjects_returns_students_applications()
    {
        var projects = await _repo.GetAppliedProjects(2);

        var expectedList = new List<SimplifiedProjectDTO>{new SimplifiedProjectDTO(1,"Title1"), new SimplifiedProjectDTO(2, "Title2")};

        Assert.Equal(expectedList,projects);
    }

    [Fact]

    public async Task ShowListOfAppliedProjects_returns_empty_list_if_student_has_no_applications()
    {
        var projects = await _repo.GetAppliedProjects(4);

        Assert.Equal(new List<SimplifiedProjectDTO>{},projects);
    }

    [Fact]
    public async Task SelectNrOfProjectApplications_returns_2_if_project_has_two_applications()
    {
        var applications = await _repo.GetNrOfProjectApplications(2);

        Assert.Equal(2,2);


    }

    [Fact]
    public async Task SelectNrOfProjectApplications_returns_0_as_default_is_project_has_no_applications()
    {
        var applications = await _repo.GetNrOfProjectApplications(4);

        Assert.Equal(0,0);
    }

    [Fact]
    public async Task DeleteApplications_returns_deleted_if_applications_for_a_project_have_been_deleted()
    {
        var response = await _repo.DeleteApplications(1);

        Assert.Equal(Deleted,response);
    }

    [Fact]
    public async Task DeleteApplications_returns_NotFound_if_project_has_no_applications()
    {
        var response = await _repo.DeleteApplications(4);

        Assert.Equal(NotFound,response);
    }

    [Fact]

    public async Task AddView_returns_created_if_student_had_not_previously_viewed_the_project()
    {
        var response = await _repo.AddView(3,4);

        Assert.Equal(Created,response);
    }

    [Fact]
    public async Task AddView_returns_conflict_if_student_has_already_viewed_the_project()
    {
        var response = await _repo.AddView(1,2);

        Assert.Equal(Conflict,response);
    }

    [Fact]

    public async Task GetViewsOfProject_returns_2_if_two_users_have_viewed_the_project()
    {
        var response = await _repo.GetViewsOfProject(1);

        Assert.Equal(2,2);
    }

    [Fact]

    public async Task GetViewsOfProjectReturns_0_If_no_users_have_viewed_the_project()
    {
        var project = _repo.CreateProject("temp project","body", 1);
        var response = await _repo.GetViewsOfProject(3);

        Assert.Equal(0,0);
    }

    [Fact]

    public async Task DeleteViews_returns_deleted_if_views_of_project_have_been_deleted()
    {
        var response = await _repo.DeleteViews(1);

        Assert.Equal(Deleted,response);
    }
    [Fact]

    public async Task DeleteViews_returns_NotFound_if_project_has_no_views()
    {
        var response = await _repo.DeleteViews(4);

        Assert.Equal(NotFound,response);
    }
    [Fact]
    public async Task CreateUser_returns_created_and_new_user_if_no_user_in_database_with_provided_email()
    {
        var created = await _repo.CreateUser("Karl", false, "Email@mei.dk");

        Assert.Equal((Created, new UserDTO(5, "Karl", false, "Email@mei.dk")), created);
    }
    [Fact]
    public async Task CreateUser_given_existing_email_returns_Conflict_and_user()
    {
        var created = await _repo.CreateUser("jakob", false, "jakob@");

        Assert.Equal((Conflict, new UserDTO(2, "jakob", false, "jakob@")), created);
    }
    [Fact]
    public async Task GetUser_returns_requested_user()
    {
        var user = await _repo.GetUser(2);

        Assert.Equal(new UserDTO(2, "jakob", false, "jakob@"), user);
    }
    [Fact]
    public async Task GetUser_returns_null_if_no_user_with_provided_id_exists()
    {
        var user = await _repo.GetUser(22);

        Assert.Null(user);
    }
    [Fact]
    public async Task GetUserWithEmail_returns_requested_user()
    {
        var user = await _repo.GetUserByEmail("jakob@");

        Assert.Equal(new UserDTO(2, "jakob", false, "jakob@"), user);
    }
    [Fact]
    public async Task GetUserWithEmail_returns_null_if_user_doesnt_exists()
    {
        var user = await _repo.GetUserByEmail("whateven@");

        Assert.Null(user);
    }







}

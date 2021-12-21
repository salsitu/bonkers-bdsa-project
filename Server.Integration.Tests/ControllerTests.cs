using ProjectBank.Server.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

//Inspired by https://github.com/ondfisk/BDSA2021/blob/main/MyApp.Server.Integration.Tests/CharacterTests.cs

namespace Server.Integration.Tests;

public class ControllerTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public ControllerTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new Microsoft.AspNetCore.Mvc.Testing.WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task List_returns_all_projects()
    {
        var projects = await _client.GetFromJsonAsync<List<Project>>("Project/List");

        var expected = new List<Project>();
        expected.Add(new Project { Id = 1, Name = "Title1" });
        expected.Add(new Project { Id = 2, Name = "Title2" });


        Assert.Contains(projects, p => p.Name == "Title1" && p.Id == 1);
        Assert.Contains(projects, p => p.Name == "Title2" && p.Id == 2);
        Assert.True(condition: projects.Count >= 2);

    }

    [Fact]
    public async Task Post_creates_project_if_name_is_unique()
    {
        var project = new Project();
        project.Name = "CreatedTitle";
        project.Description = "CreatedBody";
        project.AuthorId = 1;


        var response = await _client.PostAsJsonAsync("Project/Post", project);
        var foundProject = await _client.GetFromJsonAsync<Project>("Project/1");
        var expectedName = "Title1";
        var expectedId = 1;

        Assert.True(response.IsSuccessStatusCode == true);
        Assert.Equal(expectedId, foundProject.Id);
        Assert.Equal(expectedName, foundProject.Name);

    }

    [Fact]
    public async Task Post_does_not_create_project_if_name_is_duplicate()
    {
        var project = new Project();
        project.Name = "Title1";
        project.Description = "CreatedBody";
        project.AuthorId = 1;


        var response = await _client.PostAsJsonAsync("Project/Post", project);
        var projects = await _client.GetFromJsonAsync<List<Project>>("Project/List");


        Assert.False(response.IsSuccessStatusCode);
        Assert.DoesNotContain(projects, p => p.Name == "Title1" && p.AuthorId == 1);

    }

    [Fact]
    public async Task Project_Id_returns_requested_project()
    {
        var project = await _client.GetFromJsonAsync<Project>("Project/1");

        var expectedName = "Title1";
        var expectedId = 1;

        Assert.Equal(expectedId, project.Id);
        Assert.Equal(expectedName, project.Name);
    }

    [Fact]
    public async Task Project_Student_returns_list_of_projects_student_has_applied_to()
    {
        var projects = await _client.GetFromJsonAsync<List<SimplifiedProjectDTO>>($"Project/Student/2");

        var expected = new List<SimplifiedProjectDTO>();
        expected.Add(new SimplifiedProjectDTO(1, "Title1"));
        expected.Add(new SimplifiedProjectDTO(2, "Title2"));

        Assert.Equal(expected, projects);
    }

    [Fact]

    public async Task Project_Author_returns_list_of_projects_supervisor_has_created()
    {
        var projects = await _client.GetFromJsonAsync<List<SimplifiedProjectDTO>>($"Project/Author/1");
        var expected = new List<SimplifiedProjectDTO>();
        expected.Add(new SimplifiedProjectDTO(1, "Title1"));

        Assert.Equal(expected, projects);
    }



    [Fact]
    public async Task Project_Delete_deletes_given_project()
    {
        //in case there is currently no project with id 3 in database depending on test ordering
        var project = new Project();
        project.Name = "ProjectToBeDeleted";
        project.Description = "Body";
        project.AuthorId = 5;
        var created = await _client.PostAsJsonAsync("Project/Post", project);


        var projectToBeDeleted = await _client.GetFromJsonAsync<Project>("Project/3");
        var deleted = await _client.DeleteAsync("Project/Delete/3");
        var projects = await _client.GetFromJsonAsync<List<Project>>("Project/List");


        Assert.DoesNotContain(projects, p => p.Id == projectToBeDeleted.Id);
    }

    [Fact]
    public async Task Email_returns_user_associated_with_email()
    {
        var user = await _client.GetFromJsonAsync<UserDTO>($"Project/Email/paolo@");

        var expected = new UserDTO(1, "paolo", true, "paolo@");

        Assert.Equal(expected, user);

    }

    [Fact]
    public async Task GetViews_returns_views_associated_with_given_project_id()
    {
        var views = await _client.GetFromJsonAsync<int>("Project/GetViews/1");

        Assert.True(views >= 2);

    }

    [Fact]
    public async Task PutView_increases_the_number_of_views_bye_one_for_given_project_id_if_student_not_counted_before()
    {
        var studentId = 4;
        var response = await _client.PutAsJsonAsync($"Project/PutView/1", studentId);
        var views = await _client.GetFromJsonAsync<int>("Project/GetViews/1");

        Assert.Equal(3, views);
    }

    [Fact]
    public async Task PutView_does_not_increase_the_number_of_views_for_a_given_project_if_student_has_viewed_it_before()
    {
        var studentId = 2;
        var viewsBeforePut = await _client.GetFromJsonAsync<int>("Project/GetViews/1");

        var response = await _client.PutAsJsonAsync($"Project/PutView/1", studentId);
        var viewsAfterPut = await _client.GetFromJsonAsync<int>("Project/GetViews/1");

        Assert.Equal(viewsBeforePut, viewsAfterPut);

    }

    [Fact]

    public async Task DeleteView_deletes_all_views_associated_with_a_project()
    {
        var response = await _client.DeleteAsync("Project/DeleteView/2");

        var views = await _client.GetFromJsonAsync<int>("Project/GetViews/2");

        Assert.Equal(0, views);
    }



    [Fact]
    public async Task Applications_returns_number_of_applicants_for_given_project()
    {
        var applications = await _client.GetFromJsonAsync<int>("Project/Applications/1");

        Assert.Equal(2, applications);
    }

    [Fact]
    public async Task Apply_increases_number_of_applications_by_one_if_student_is_new_applicant()
    {
        var studentID = 2;
        var response = await _client.PutAsJsonAsync("Project/Apply/2", studentID);
        var applications = await _client.GetFromJsonAsync<int>("Project/Applications/2");

        Assert.Equal(1, applications);
    }

    [Fact]
    public async Task Apply_does_not_increase_number_of_applications_by_one_if_student_has_already_applied()
    {
        var studentID = 2;
        var response = await _client.PutAsJsonAsync("Project/Apply/2", studentID);
        var applications = await _client.GetFromJsonAsync<int>("Project/Applications/1");

        Assert.Equal(2, applications);
    }

    [Fact]
    public async Task DeleteApplication_removes_all_applications_associated_with_given_project_id()
    {
        //in case there is no project 3 in database due to test sequence
        var project = new Project();
        project.Name = "ProjectToBeDeleted";
        project.Description = "Body";
        project.AuthorId = 5;
        var created = await _client.PostAsJsonAsync("Project/Post", project);

        //adding application to ensure result is not just default, but applicants were actually deleted after call
        var studentID = 2;
        var response = await _client.PutAsJsonAsync("Project/Apply/3", studentID);

        var deleteResponse = await _client.DeleteAsync($"Project/DeleteApplication/3");
        var applications = await _client.GetFromJsonAsync<int>("Project/Applications/3");

        Assert.Equal(0, applications);

    }

    [Fact]

    public async Task IsApplied_returns_true_if_student_with_id_has_already_applied_to_project()
    {
        var alreadyApplied = await _client.GetFromJsonAsync<bool>($"Project/IsApplied/1/2");

        Assert.True(alreadyApplied);
    }

    [Fact]
    public async Task IsApplied_returns_false_if_student_with_id_has_not_applied_to_project()
    {
        var alreadyApplied = await _client.GetFromJsonAsync<bool>($"Project/IsApplied/1/4");

        Assert.False(alreadyApplied);
    }
}
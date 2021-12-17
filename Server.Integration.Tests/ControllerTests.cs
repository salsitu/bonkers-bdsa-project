using ProjectBank.Server.Entities;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

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
    public async  Task List_returns_all_projects()
    {
        var projects = await _client.GetFromJsonAsync<List<Project>>("Project/List");

        var expected = new List<Project>();
        expected.Add(new Project { Id = 1, Name = "huhu"});
        expected.Add(new Project { Id = 2, Name = "hihi"});

        Assert.Equal(expected[0].Name,projects[0].Name);
        Assert.Equal(expected[0].Id, projects[0].Id);
        Assert.Equal(expected[1].Name, projects[1].Name);
        Assert.Equal(expected[1].Id, projects[1].Id);

        

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
        var expectedName = "huhu";
        var expectedId = 1;

        Assert.True(response.IsSuccessStatusCode == true);
        Assert.Equal(expectedId, foundProject.Id);
        Assert.Equal(expectedName, foundProject.Name);

    }

    [Fact]
    public async Task Post_does_not_create_project_if_name_is_duplicate()
    {
        var project = new Project();
        project.Name = "hihi";
        project.Description = "CreatedBody";
        project.AuthorId = 1;


        var response = await _client.PostAsJsonAsync("Project/Post", project);

        Assert.True(response.IsSuccessStatusCode == true);

        var projects = await _client.GetFromJsonAsync<List<Project>>("Project/List");
        Assert.Equal(2, actual: projects.Count);
    }

    [Fact]
    public async Task Project_Id_returns_requested_project()
    {
        var request = await _client.GetFromJsonAsync<Project>("Project/1");

        var expectedName = "huhu";
        var expectedId = 1;
        
        Assert.Equal(expectedId, request.Id);
        Assert.Equal(expectedName, request.Name);
    }

    [Fact]
    public async Task Project_Student_returns_list_of_projects_student_has_applied_to()
    {
        var projects = await _client.GetFromJsonAsync<List<SimplifiedProjectDTO>>($"Project/Student/2");

        var expected = new List<SimplifiedProjectDTO>();
        expected.Add(new SimplifiedProjectDTO(1, "huhu"));
        expected.Add(new SimplifiedProjectDTO(2, "hihi"));

        Assert.Equal(expected, projects);
    }

    [Fact]

    public async Task Project_Author_returns_list_of_projects_supervisor_has_created() 
    {
        var projects = await _client.GetFromJsonAsync<List<SimplifiedProjectDTO>>($"Project/Author/1");
        var expected = new List<SimplifiedProjectDTO>();
        expected.Add(new SimplifiedProjectDTO(1, "huhu"));

        Assert.Equal(expected, projects);
    }

    /*

    [Fact]
    public async Task Project_Delete_deletes_given_project()
    {
        var response = await _client.DeleteAsync("Project/Delete/1");
        var projects = await _client.GetFromJsonAsync<List<Project>>("Project/List");
        var expectedName = "hihi";
        var expectedId = 2;

        Assert.Equal(expectedId, projects[0].Id);
        Assert.Equal(expectedName, projects[0].Name);
        Assert.Single(projects);


    }
    */

}
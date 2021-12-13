using System;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Microsoft.Extensions.Logging;
using static ProjectBank.Server.Entities.Response;
using ProjectBank.Server.Controllers;
using ProjectBank.Server.Entities;
using Server.Entities;
using System.Collections.Generic;

namespace Server.Tests;

public class ProjectControllerTests
{
    [Fact]
    public async void Get_Returns_Project_When_Given_Id ()
    {
        var expected = new ProjectDTO(1, "Project", "Body of project", 1);
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.SelectProject(1)).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.Get(1);

        Assert.Equal(expected, (actual.Result as OkObjectResult).Value);
    }

    [Fact]
    public async void Get_Returns_NotFound_When_Given_NonExisting_Project()
    {
        var expected = new ProjectDTO(1, "Project", "Body of project", 1);
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.SelectProject(2)).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.Get(1);

        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async void GetByAuthor_Returns_EmptyList_When_Given_AuthorId_With_No_Created_Projects()
    {
        var expected = Array.Empty<SimplifiedProjectDTO>();
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.ShowCreatedProjects(1)).ReturnsAsync(() => new List<SimplifiedProjectDTO>(expected));
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetByAuthor(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void GetByAuthor_Returns_CreatedProject_When_Given_AuthorId_With_Created_Projects()
    {
        var project1 = new SimplifiedProjectDTO(1, "Title1");
        var project2 = new SimplifiedProjectDTO(2, "Title2");
        var expected = new List<SimplifiedProjectDTO>();
        expected.Add(project1);
        expected.Add(project2);
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.ShowCreatedProjects(1)).ReturnsAsync(() => new List<SimplifiedProjectDTO>(expected));
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetByAuthor(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void AddView_Returns_Created_When_Given_ProjectId_And_StudentId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.AddView(1, 1)).ReturnsAsync(Response.Created);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.AddView(1, 1);

        Assert.Equal(Response.Created, actual);
    }

    [Fact]
    public async void AddView_Returns_Conflict_When_Given_Existing_ProjectId_And_StudentId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.AddView(1, 1)).ReturnsAsync(Response.Conflict);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.AddView(1, 1);

        Assert.Equal(Response.Conflict, actual);
    }

    [Fact]
    public async void DeleteApplication_Returns_Deleted_When_Given_ProjectId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.DeleteApplications(1)).ReturnsAsync(Response.Deleted);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.DeleteApplication(1);

        Assert.Equal(Response.Deleted, actual);
    }

    [Fact]
    public async void DeleteApplication_Returns_NotFound_When_Given_NonExisting_ProjectId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.DeleteApplications(1)).ReturnsAsync(Response.NotFound);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.DeleteApplication(1);

        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public async void DeleteView_Returns_Deleted_When_Given_ProjectId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.DeleteViews(1)).ReturnsAsync(Response.Deleted);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.DeleteView(1);

        Assert.Equal(Response.Deleted, actual);
    }

    [Fact]
    public async void DeleteView_Returns_NotFound_When_Given_NonExisting_ProjectId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.DeleteViews(1)).ReturnsAsync(Response.NotFound);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.DeleteView(1);

        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public async void Delete_Returns_Deleted_When_Given_ProjectId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.DeleteProject(1)).ReturnsAsync(Response.Deleted);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.Delete(1);

        Assert.Equal(Response.Deleted, actual);
    }

    [Fact]
    public async void Delete_Returns_NotFound_When_Given_NonExisting_ProjectId()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.DeleteProject(1)).ReturnsAsync(Response.NotFound);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.Delete(1);

        Assert.Equal(Response.NotFound, actual);
    }

    [Fact]
    public async void Post_Returns_Created_When_Given_Project()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.CreateProject("Project", "Desc", 1)).ReturnsAsync(() => (Response.Created, new ProjectDTO(1, "Project", "Desc", 1)));
        var controller = new ProjectController(repository.Object);

        var project = new Project { Id = 1, Name = "Project", Description = "Desc", AuthorId = 1};
        var expected = (Response.Created, new ProjectDTO(1, "Project", "Desc", 1));
        var actual = await controller.Post(project);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void Post_Returns_Conflict_When_Given_Existing_Project()
    {
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.CreateProject("Project", "Desc", 1)).ReturnsAsync(()=> (Response.Conflict, new ProjectDTO(1, "Project", "Desc", 1)));
        var controller = new ProjectController(repository.Object);

        var project = new Project { Id = 1, Name = "Project", Description = "Desc", AuthorId = 1 };
        var expected = (Response.Conflict, new ProjectDTO(1, "Project", "Desc", 1));
        var actual = await controller.Post(project);

        Assert.Equal(expected, actual);
    }
}

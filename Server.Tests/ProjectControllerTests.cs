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
    public async void AddView_Returns_CreatedProject_When_Given_AuthorId_With_Created_Projects()
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
}

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
    public async void GetByEmail_Returns_User_When_Given_Email()
    {
        var expected = new UserDTO(1, "Name", true, "email@test.com");
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.GetUserByEmail("email@test.com")).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetByEmail("email@test.com");

        Assert.Equal(expected, (actual.Result as OkObjectResult).Value);
    }

    [Fact]
    public async void GetByEmail_Returns_User_When_Given_Email1()
    {
        var expected = new UserDTO(1, "Name", true, "email@test.com");
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.GetUserByEmail("email@test.com")).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetByEmail("wrong@email.com");

        Assert.IsType<NotFoundResult>(actual.Result);
    }

    [Fact]
    public async void ShowListOfAppliedProjects_Returns_List_Of_Applied_Projects_When_Given_UserID()
    {
        var project1 = new SimplifiedProjectDTO(1, "Title1");
        var project2 = new SimplifiedProjectDTO(2, "Title2");
        var expected = new List<SimplifiedProjectDTO>();
        expected.Add(project1);
        expected.Add(project2);
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.ShowListOfAppliedProjects(1)).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.ShowListOfAppliedProjects(1);

        Assert.Equal(expected, actual);
    }
    [Fact]
    public async void ShowListOfAppliedProjects_Returns_Empty_List_Of_Projects_When_Given_UserID_With_No_Applications()
    {
        
        var expected = Array.Empty<SimplifiedProjectDTO>();

        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.ShowListOfAppliedProjects(1)).ReturnsAsync(() => new List<SimplifiedProjectDTO>(expected));
        var controller = new ProjectController(repository.Object);

        var actual = await controller.ShowListOfAppliedProjects(1);

        Assert.Equal(expected, actual);
    }
    [Fact]
    public async void Get_Returns_Empty_Array_When_No_Projects_Exist()
    {

        var expected = Array.Empty<SimplifiedProjectDTO>();

        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.ShowAllProjects()).ReturnsAsync(() => new List<SimplifiedProjectDTO>(expected));
        var controller = new ProjectController(repository.Object);

        var actual = await controller.Get();

        Assert.Equal(expected, actual); 
    }

    [Fact]
    public async void Get_Returns_All_Projects()
    {
        var project1 = new SimplifiedProjectDTO(1, "Title1");
        var project2 = new SimplifiedProjectDTO(2, "Title2");
        var expected = new List<SimplifiedProjectDTO>();
        expected.Add(project1);
        expected.Add(project2);
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.ShowAllProjects()).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.Get();

        Assert.Equal(expected, actual);
    }
    [Fact]
    public async void GetViews_Returns_View_Of_Project()
    {
        var expected = 1;
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.GetViewsOfProject(1)).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetViews(1);

        Assert.Equal(expected, actual);
    }
    [Fact]
    public async void GetViews_Returns_0_As_Default()
    {
        var expected = 0;
        var repository = new Mock<IDBFacade>();
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetViews(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void GetApplications_Returns_Applications_Of_Project()
    {
        var expected = 1;
        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.SelectNrOfProjectApplications(1)).ReturnsAsync(expected);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetApplications(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void GetApplications_Returns_0_As_Default()
    {
        var expected = 0;
        var repository = new Mock<IDBFacade>();
        var controller = new ProjectController(repository.Object);

        var actual = await controller.GetApplications(1);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void IsApplied_Returns_True_When_Student_Is_Applied_To_Given_Project()
    {
        var expected = true;
        var response = Exists; 
        var projectid = 1;
        var studentid = 1;

        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.HasAlreadyAppliedToProject(projectid, studentid)).ReturnsAsync(response);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.IsApplied(studentid,projectid);

        Assert.Equal(expected, actual);
    }

    [Fact]
    public async void IsApplied_Returns_False_When_Student_Is_Not_Applied_To_Given_Project()
    {
        var expected = false;
        var response = NotFound;
        var projectid = 1;
        var studentid = 1;

        var repository = new Mock<IDBFacade>();
        repository.Setup(m => m.HasAlreadyAppliedToProject(projectid, studentid)).ReturnsAsync(response);
        var controller = new ProjectController(repository.Object);

        var actual = await controller.IsApplied(studentid, projectid);

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

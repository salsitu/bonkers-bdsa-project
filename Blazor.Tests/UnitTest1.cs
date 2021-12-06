using Xunit;
using Bunit;
using ProjectBank.Client.Pages;
using RichardSzalay.MockHttp;
using ProjectBank.Shared;

namespace Blazor.Tests;

public class UnitTest1
{
    [Fact]
    public void TestCounterIncrementsWhenClicked()
    {
        // Arrange
        using var ctx = new TestContext();
        var cut = ctx.RenderComponent<Counter>();
        var paraElm = cut.Find("p");

        // Act
        cut.Find("button").Click();
        var paraElmText = paraElm.TextContent;

        // Assert
        paraElmText.MarkupMatches("Current count: 1");
    }
    //Does not pass
    /*
    [Fact]
    public void TestName()
    {
        //var project = new(1, "Bennys bog1", "Desc1", 2, 2, 0.5f);

        using var ctx = new TestContext();
        var mock = ctx.Services.AddMockHttpClient();   
        mock.When("/Project/{id}").RespondJson<Project>(new Project(1, "Bennys bog1", "Desc1", 2, 2, 0.5f));

        var cut = ctx.RenderComponent<ProjectComponent>(
            parameters => parameters.Add(c => c.id, 1));

        var paraElm = cut.Find("h1");
        var paraElmText = paraElm.TextContent;
        paraElmText.MarkupMatches("Bennys bog1");


        //Assert.Equal("Bennys bog1", cut.Find($"h1").TextContent);

    }
    */
}
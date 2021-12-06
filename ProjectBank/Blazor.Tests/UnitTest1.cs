using Xunit;
using Bunit;
using ProjectBank.Client.Pages;
using ProjectBank.Server.Controllers;

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

    /*
    [Fact]
    public void TestName()
    {
        //var project = new(1, "Bennys bog1", "Desc1", 2, 2, 0.5f);

        using var ctx = new TestContext();
        ctx.Services.AddFallbackServiceProvider

            Add<IWeatherForecastService>(new ProjectController());

        var cut = ctx.RenderComponent<ProjectComponent>(
            parameters => parameters.Add(c => c.id, 1));

        Assert.Equal("Bennys bog1", cut.Find($"span").TextContent);

    }*/
}
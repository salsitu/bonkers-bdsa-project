using ProjectBank.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;


namespace ProjectBank.Server.Controllers;


[ApiController]
[Route("[controller]")]

public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    //private readonly ICharacterRepository _repository;

    public ProjectController(ILogger<ProjectController> logger)
    {
        _logger = logger;
    }

    /*
    [AllowAnonymous]
    [HttpGet]
    public async Task<IReadOnlyCollection<ProjectDTO>> Get()
        => await _repository.ReadAsync();
    */

 
    [HttpGet]
    public IEnumerable<Project> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new Project
        {
            Title = "Test",
            Description = "testing"
        })
       .ToArray();
    }
}
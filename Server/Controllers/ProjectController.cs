using ProjectBank.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBank.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    public List<Project> Projects = new()
    {
        new(1, "Bennys bog1", "Desc1", 2, 2, 0.5f),
        new(2, "Bennys bog2", "Desc2", 2, 2, 0.5f),
        new(3, "Bennys bog3", "Desc3", 2, 2, 0.5f),
        new(4, "Bennys bog4", "Desc4", 2, 2, 0.5f)
    };

    private List<Supervisor> Supervisors = new()
    {
        new(1, "Benny", new List<Project>() { new Project(1, "Bennys bog", "Bennys beskrivelse", 2, 2, 0.5f) }),
        new(2, "Fanny", new List<Project>() { new Project(1, "Fannys bog", "Fannys beskrivelse", 2, 2, 0.5f) }),
        new(3, "Dorte", new List<Project>() { new Project(1, "Dortes bog", "Dortes beskrivelse", 2, 2, 0.5f) }),
        new(4, "Bentes", new List<Project>() { new Project(1, "Bentes bog", "Bentes beskrivelse", 2, 2, 0.5f) }),
    };


    [HttpGet("{id}")]
    public ActionResult Get(int id)
    {
        var project = Projects.Find(o => o.id == id);
        if (project != null)
        {
            return Ok(project);
        }
        return NotFound();
    }

    [HttpGet("Author/{authorid}")]
    public ActionResult GetByAuthor(int authorid)
    {
        var author = Supervisors.Find(o => o.Id == authorid);
        if (author != null)
        {
            return Ok(author.CreatedProjects);
        }
        return NotFound();
    }


    [HttpGet("List")]
    public ActionResult Get()
    {
        return Ok(Projects);
    }


    [HttpGet("Views/{id}")]
    public ActionResult GetViews(int projectid)
    {
        Console.WriteLine("Me get views");
        return Ok(22);
    }

    [HttpGet("Applications/{id}")]
    public ActionResult GetApplications(int projectid)
    {
        Console.WriteLine("Trying to get applic");
        return Ok(2);
    }

    [HttpPut("Apply/{projectId}")]
    public ActionResult ApplyForProject(int projectid, [FromBody] int studentid)
    {
        return Ok(); //Return enum, when merging with DB
    }

    [HttpPost("Post")]
    public ActionResult Post(Project project)
    {
        var author = Supervisors.Find(o => o.Id == 2);
        author.CreatedProjects.Add(project);

        return Ok(author.CreatedProjects);
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var project = Projects.Find(o => o.id == id);
        if(project != null)
        {
            Projects.Remove(project);
            return(Ok(project));
        }
        return NotFound();
    }
       
}


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
    private List<Project> Projects = new()
    {
        new(1, "Bennys bog1", "Desc1", "Benny", 2, 0.5f),
        new(2, "Bennys bog2", "Desc2", "Benny", 2, 0.5f),
        new(3, "Bennys bog3", "Desc3", "Benny", 2, 0.5f),
        new(4, "Bennys bog4", "Desc4", "Benny", 2, 0.5f)
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
    
    [HttpGet("List")]
    public ActionResult Get()
    {
        return Ok(Projects);
    }
}
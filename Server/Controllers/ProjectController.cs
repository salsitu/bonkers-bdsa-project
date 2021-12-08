using ProjectBank.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectBank.Server.Entities;
using Microsoft.EntityFrameworkCore;

namespace ProjectBank.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private DBFacade _repository;

    public ProjectController(DBFacade repository)
    {
        _repository = repository;          
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO),StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        var project = await _repository.SelectProject(id);

        if (project != null)
        {   
            return Ok(project);
        }
        return NotFound();
    }

    [HttpGet("Author/{authorid}")]
    public async Task<List<SimplifiedProjectDTO>> GetByAuthor(int authorid)
    {
        return await _repository.ShowCreatedProjects(authorid);
    }

    [HttpGet("Email/{email}")]
    public async Task<UserDTO> GetByEmail(string email)
    {
        return await _repository.GetUserByEmail(email);
    }


    [HttpGet("List")]
    public async Task<List<SimplifiedProjectDTO>> Get()
    {
        return await _repository.ShowAllProjects();
    }


    [HttpGet("Views/{id}")]
    public async Task<int> GetViews(int projectid)
    {
        return await _repository.GetViewsOfProject(projectid);
    }

    [HttpGet("Applications/{id}")]
    public async Task<int> GetApplications(int projectid)
    {
        return await _repository.SelectNrOfProjectApplications(projectid);  
    }

    [HttpPut("Apply/{projectId}")]
    public async Task<ActionResult> ApplyForProject(int projectid, [FromBody] int studentid)
    {
        var response = await _repository.ApplyToProject(projectid, studentid);

        switch (response)
        {
            case Entities.Response.Created: return Ok();

            case Entities.Response.Updated: return Ok();
            
            case Entities.Response.Deleted: return NoContent();

            case Entities.Response.NotFound: return NotFound();
            
            case Entities.Response.BadRequest: return BadRequest();

            case Entities.Response.Conflict: return Conflict();

            default: return NotFound();
        }
    }
  

    [HttpPost("Post")]
    [ProducesResponseType(typeof(ProjectDTO), 201)]
    public async Task<(Response, ProjectDTO)> Post(Project project) 
    {
        return await _repository.CreateProject(project.Name, project.Description, project.AuthorId);
    }
    

    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Response> Delete(int id)
    {
        return await _repository.DeleteProject(id);
    }
}


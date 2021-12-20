using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProjectBank.Server.Entities;
using Microsoft.EntityFrameworkCore;
using Server.Entities;

namespace ProjectBank.Server.Controllers;

[Route("[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private IDBFacade _repository;

    public ProjectController(IDBFacade repository)
    {
        _repository = repository;
    }


    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProjectDTO), StatusCodes.Status200OK)]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        var project = await _repository.GetProject(id);

        if (project != null)
        {
            return Ok(project);
        }
        return NotFound();
    }

    [HttpGet("Author/{authorid}")]
    public async Task<List<SimplifiedProjectDTO>> GetByAuthor(int authorid)
    {
        return await _repository.GetCreatedProjects(authorid);
    }

    [HttpGet("Email/{email}")]
    public async Task<ActionResult<UserDTO>> GetByEmail(string email)
    {
        var user = await _repository.GetUserByEmail(email);

        if (user != null)
        {
            return Ok(user);
        }
        return NotFound();
    }
    [HttpGet("Student/{studentid}")]
    public async Task<List<SimplifiedProjectDTO>> GetAppliedProjects(int studentId)
    {
        return await _repository.GetAppliedProjects(studentId);
    }

    [HttpGet("List")]
    public async Task<List<SimplifiedProjectDTO>> Get()
    {
        return await _repository.GetAllProjects();
    }


    [HttpGet("GetViews/{projectid}")]
    public async Task<int> GetViews(int projectid)
    {
        return await _repository.GetViewsOfProject(projectid);
    }

    [HttpGet("Applications/{projectid}")]
    public async Task<int> GetApplications(int projectid)
    {
        return await _repository.GetNrOfProjectApplications(projectid);
    }

    [HttpGet("IsApplied/{projectid}/{studentid}")]
    public async Task<bool> IsApplied(int projectid, int studentid)
    {
        var x = await _repository.HasAlreadyAppliedToProject(projectid, studentid);
        if (x == Entities.Response.Exists) return true;
        else return false;
    }

    [HttpPut("Apply/{projectId}")]
    public async Task<ActionResult> ApplyForProject(int projectid, [FromBody] int studentid)
    {
        var response = await _repository.ApplyToProject(projectid, studentid);

        if (response == Entities.Response.Created)
        {
            return Ok();
        }
        return Conflict();
    }


    [HttpPost("Post")]
    [ProducesResponseType(typeof(ProjectDTO), 201)]
    public async Task<ActionResult> Post(Project project)
    {
        var x = await _repository.CreateProject(project.Name, project.Description, project.AuthorId);

        if (x.Item1 == Entities.Response.Conflict)
        {
            return Conflict();
        }
        return Ok();
    }


    [HttpDelete("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<Response> Delete(int id)
    {
        return await _repository.DeleteProject(id);
    }

    [HttpDelete("DeleteView/{projectid}")]
    public async Task<Response> DeleteView(int projectid)
    {
        return await _repository.DeleteViews(projectid);
    }

    [HttpDelete("DeleteApplication/{projectid}")]
    public async Task<Response> DeleteApplication(int projectid)
    {
        return await _repository.DeleteApplications(projectid);
    }


    [HttpPut("PutView/{id}")]
    public async Task<Response> AddView(int id, [FromBody] int studentId)
    {
        return await _repository.AddView(id, studentId);
    }
}


using ProjectBank.Server.Entities;
using System;

namespace ProjectBank.Server.Entities;

public interface IDBFacade
{
    public Task<(Response, ProjectDTO)> CreateProject(string name, string description, int authorId);
    public Task<int> GetUserIdByEmail(string email);
    public Task<UserDTO> GetUserByEmail(string email);
    public Task<UserDTO> GetUser(int id);
    public Task<(Response, UserDTO)> CreateUser(string name, bool isSupervisor, string email);
    public Task<Response> DeleteViews(int projectId);
    public Task<int> GetViewsOfProject(int projectId);
    public Task<Response> AddView(int projectId, int studentId);
    public Task<Response> DeleteApplications(int projectId);
    public Task<int> SelectNrOfProjectApplications(int projectId);
    public Task<List<SimplifiedProjectDTO>> ShowListOfAppliedProjects(int studentId);
    public Task<Response> HasAlreadyAppliedToProject(int projectid, int studentId);
    public Task<Response> ApplyToProject(int projectId, int studentId);
    public Task<List<SimplifiedProjectDTO>> ShowCreatedProjects(int authorId);
    public Task<List<SimplifiedProjectDTO>> ShowAllProjects();
    public Task<ProjectDTO> SelectProject(int projectId);
    public Task<Response> DeleteProject(int projectId);
}


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
    
    public Task<int> GetNrOfProjectApplications(int projectId);
    
    public Task<List<SimplifiedProjectDTO>> GetAppliedProjects(int studentId);
    
    public Task<Response> HasAlreadyAppliedToProject(int projectid, int studentId);
    
    public Task<Response> ApplyToProject(int projectId, int studentId);
    
    public Task<List<SimplifiedProjectDTO>> GetCreatedProjects(int authorId);
    
    public Task<List<SimplifiedProjectDTO>> GetAllProjects();
    
    public Task<ProjectDTO> GetProject(int projectId);
    
    public Task<Response> DeleteProject(int projectId);
}


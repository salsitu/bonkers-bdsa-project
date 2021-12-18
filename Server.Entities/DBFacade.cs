using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Entities.Response;
using ProjectBank.Server;
using Server.Entities;

namespace ProjectBank.Server.Entities
{
    public class DBFacade : IDBFacade
    {
        private readonly ProjectBankContext _context;
        private readonly IProjectRepository _projectRepo;
        private readonly IApplicantRepository _applicantRepo;
        private readonly IViewRepository _viewRepo;
        private readonly IUserRepository _userRepo;
        public DBFacade(ProjectBankContext context)
        {
            _context = context;
            _projectRepo = new ProjectRepository(context);
            _applicantRepo = new ApplicantRepository(context);
            _viewRepo = new ViewRepository(context);
            _userRepo = new UserRepository(context);
        }

        public Task<(Response, ProjectDTO)> CreateProject(string name, string description, int authorId)
        {
            var project = new ProjectCreateDTO(name, description, authorId);

            return _projectRepo.CreateAsync(project);
        }

        public Task<ProjectDTO> GetProject(int projectId)
        {
            return _projectRepo.GetProjectAsync(projectId);
        }

        public Task<Response> DeleteProject(int projectId)
        {
            return _projectRepo.DeleteAsync(projectId);
        }

        public Task<List<SimplifiedProjectDTO>> GetAllProjects()
        {
            return _projectRepo.GetAllProjectsAsync();
        }
        
        public Task<List<SimplifiedProjectDTO>> GetCreatedProjects(int authorId)
        {
            return _projectRepo.GetCreatedProjectsAsync(authorId);
        }

        public Task<Response> ApplyToProject(int projectId, int studentId)
        {
            return _applicantRepo.ApplyToProjectAsync(projectId, studentId);
        }

        public Task<Response> HasAlreadyAppliedToProject(int projectid, int studentId)
        {
            return _applicantRepo.HasAlreadyAppliedToProjectAsync(projectid,studentId);
        }

        public Task<List<SimplifiedProjectDTO>> GetAppliedProjects(int studentId)
        {
            return _applicantRepo.GetAppliedProjectsAsync(studentId);
        }

        public Task<int> GetNrOfProjectApplications(int projectId)
        {
            return _applicantRepo.GetNrOfProjectApplicationsAsync(projectId);
        }

        public Task<Response> DeleteApplications(int projectId)
        {
            return _applicantRepo.DeleteApplicationsAsync(projectId);
        }

        public Task<Response> AddView(int projectId, int studentId)
        {
            return _viewRepo.AddViewAsync(projectId,studentId);
        }

        public Task<int> GetViewsOfProject(int projectId)
        {   
            Console.WriteLine("Facade =" + projectId);
            return _viewRepo.GetViewsOfProjectAsync(projectId);
        }

        public Task<Response> DeleteViews(int projectId)
        {
            return _viewRepo.DeleteViewAsync(projectId);
        }
        
        public Task<(Response, UserDTO)> CreateUser(string name, bool isSupervisor, string email)
        {
            var user = new UserCreateDTO(name, isSupervisor, email);
            return _userRepo.CreateUserAsync(user);
        }
        public Task<UserDTO> GetUser(int id)
        {
            return _userRepo.GetUserAsync(id);
        }
        public Task<UserDTO> GetUserByEmail(string email)
        {
            return _userRepo.GetUserWithEmailAsync(email);
        }
        public Task<int> GetUserIdByEmail(string email)
        {
            return _userRepo.GetUserIdWithEmailAsync(email);
        }
    }

}
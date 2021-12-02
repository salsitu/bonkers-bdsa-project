using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Response;
using ProjectBank.Server;

namespace ProjectBank.Server
{
    public class DBFacade
    {
        private readonly ProjectBankContext _context;
        private readonly ProjectRepository _projectRepo;
        private readonly ApplicantRepository _applicantRepo;
        public DBFacade(ProjectBankContext context)
        {
            _context = context;
            _projectRepo = new ProjectRepository(context);
            _applicantRepo = new ApplicantRepository(context);
        }

        public Task<(Response, ProjectDTO)> CreateProject(string name, string description, int authorId)
        {
            var project = new ProjectCreateDTO(name, description, authorId);

            return _projectRepo.CreateAsync(project);
        }

        public Task<ProjectDTO> SelectProject(int projectId)
        {
            return _projectRepo.ReadAsync(projectId);
        }

        public Task<Response> DeleteProject(int projectId)
        {
            return _projectRepo.DeleteAsync(projectId);
        }

        public Task<List<SimplifiedProjectDTO>> ShowAllProjects()
        {
            return _projectRepo.ListAllProjectsAsync();
        }

        public Task<List<SimplifiedProjectDTO>> ShowCreatedProjects(int authorId)
        {
            return _projectRepo.ShowCreatedProjectsAsync(authorId);
        }

        public Task<Response> ApplyToProject(int projectId, int StudentId)
        {
            return _applicantRepo.ApplyToProjectAsync(projectId,StudentId);
        }
    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Server.Entities
{
    internal interface IProjectRepository
    {
        public Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project);

        public Task<ProjectDTO> GetProjectAsync(int projectId);
        public Task<Response> DeleteAsync(int projectId);
        public Task<List<SimplifiedProjectDTO>> GetAllProjectsAsync();
        public Task<List<SimplifiedProjectDTO>> GetCreatedProjectsAsync(int id);

    }
}

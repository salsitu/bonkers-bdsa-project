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

        public Task<ProjectDTO> ReadAsync(int projectId);
        public Task<Response> DeleteAsync(int projectId);
        public Task<List<SimplifiedProjectDTO>> ListAllProjectsAsync();
        public Task<List<SimplifiedProjectDTO>> ShowCreatedProjectsAsync(int id);

    }
}

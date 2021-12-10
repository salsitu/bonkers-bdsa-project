using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Entities.Response;
using ProjectBank.Server;

namespace ProjectBank.Server.Entities
{
    public class ProjectRepository
    {
        private readonly ProjectBankContext _context;

        public ProjectRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public async Task<(Response, ProjectDTO)> CreateAsync(ProjectCreateDTO project)
        {
            var conflict =
                await _context.Projects
                    .Where(p => p.Name == project.Name)
                    .Select(p => new ProjectDTO(p.Id, p.Name, p.Description, p.AuthorId))
                    .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Conflict, conflict);
            }

            var entity = new Project { Name = project.Name, Description = project.Description, AuthorId = project.AuthorId };

            _context.Projects.Add(entity);

            await _context.SaveChangesAsync();

            return (Created, new ProjectDTO(entity.Id, entity.Name, entity.Description, entity.AuthorId));
        }
        public async Task<ProjectDTO> ReadAsync(int projectId)
        {
            var projects = from p in _context.Projects
                           where p.Id == projectId
                           select new ProjectDTO(p.Id, p.Name, p.Description, p.AuthorId);

            return await projects.FirstOrDefaultAsync();
        }
        public async Task<Response> DeleteAsync(int projectId)
        {
            var entity =
                await _context.Projects
                                .FirstOrDefaultAsync(p => p.Id == projectId);
            

            if (entity == null)
            {
                return NotFound;
            }

            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();

            return Deleted;
        }
        public async Task<List<SimplifiedProjectDTO>> ListAllProjectsAsync()
        {
            var projects = await (from p in _context.Projects
                           select new SimplifiedProjectDTO(p.Id, p.Name)).ToListAsync();
            return projects;
        }
        public async Task<List<SimplifiedProjectDTO>> ShowCreatedProjectsAsync(int id)
        {
            var projects = await (from p in _context.Projects
                           where p.AuthorId == id
                           select new SimplifiedProjectDTO(p.Id, p.Name)).ToListAsync();
            return projects;
        }
    }
}
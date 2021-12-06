using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Entities.Response;
using ProjectBank.Server;

namespace ProjectBank.Server.Entities
{
    public class ViewRepository
    {
        private readonly ProjectBankContext _context;

        public ViewRepository(ProjectBankContext context)
        {
            _context = context;
        }

        public async Task<Response> AddViewAsync(int projectId, int userId)
        {
            var conflict =
                await _context.Views
                .Where(v => v.ProjectId == projectId)
                .Where(v => v.StudentId == userId)
                .Select(v => new ViewCreateDTO(v.ProjectId, v.StudentId))
                .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Conflict);
            }

            var entity = new View { ProjectId = projectId, StudentId = userId };

            _context.Views.Add(entity);

            await _context.SaveChangesAsync();

            return (Created);
        }
        public async Task<int> GetViewsOfProjectAsync(int projectId)
        {
            var projects = await (from v in _context.Views
                                  where v.ProjectId == projectId
                                  select v).CountAsync();
            return projects;
        }
        public async Task<Response> DeleteViewAsync(int projectId)
        {
            var entities =
                    await (from v in _context.Views
                           where v.ProjectId == projectId
                           select new ViewCreateDTO(v.ProjectId, v.StudentId)).ToListAsync();


            if (entities.Count == 0)
            {
                return NotFound;
            }

            _context.Views.RemoveRange(_context.Views.Where(v => v.ProjectId == projectId));

            await _context.SaveChangesAsync();

            return Deleted;
        }
    }
}

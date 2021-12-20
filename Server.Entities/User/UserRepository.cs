using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Entities.Response;
using ProjectBank.Server;

namespace ProjectBank.Server.Entities
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectBankContext _context;

        public UserRepository(ProjectBankContext context)
        {
            _context = context;
        }
        public async Task<(Response, UserDTO)> CreateUserAsync(UserCreateDTO user)
        {
            var conflict =
                await _context.Users
                .Where(u => u.Email == user.Email)
                .Select(u => new UserDTO(u.Id, u.Name, u.IsSupervisor, u.Email))
                .FirstOrDefaultAsync();

            if (conflict != null)
            {
                return (Conflict, conflict);
            }
            var entity = new User { Name = user.Name, IsSupervisor = user.IsSupervisor, Email = user.Email };

            _context.Users.Add(entity);

            await _context.SaveChangesAsync();

            return (Created, new UserDTO(entity.Id, entity.Name, entity.IsSupervisor, entity.Email));
        }
        public async Task<UserDTO> GetUserAsync(int userId)
        {
            var projects = from u in _context.Users
                           where u.Id == userId
                           select new UserDTO(u.Id, u.Name, u.IsSupervisor, u.Email);

            return await projects.FirstOrDefaultAsync();
        }
        public async Task<UserDTO> GetUserWithEmailAsync(string email)
        {
            var projects = from u in _context.Users
                           where u.Email == email
                           select new UserDTO(u.Id, u.Name, u.IsSupervisor, u.Email);

            return await projects.FirstOrDefaultAsync();
        }

        public async Task<int> GetUserIdWithEmailAsync(string email)
        {
            var userId = from u in _context.Users
                         where u.Email == email
                         select u.Id;

            return await userId.FirstOrDefaultAsync();
        }
    }

}
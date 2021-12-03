using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static ProjectBank.Server.Entities.Response;
using ProjectBank.Server;

namespace ProjectBank.Server.Entities
{
	public class UserRepository
	{
        private readonly ProjectBankContext _context;

        public UserRepository(ProjectBankContext context)
        {
            _context = context;
        }
        //public async Task<(Response, UserDTO)> CreateUserAsync(UserCreateDTO user)
        //{
        //    var entity = new User { Name = user.Name, IsSupervisor = user.IsSupervisor};
        //
        //    _context.Users.Add(entity);
        //
        //    await _context.SaveChangesAsync();
        //
        //    return (Created, new UserDTO(entity.Id, entity.Name, entity.IsSupervisor));
        //}
        public async Task<UserDTO> GetUserAsync(int userId)
        {
            var projects = from u in _context.Users
                           where u.Id == userId
                           select new UserDTO(u.Id, u.Name, u.IsSupervisor);

            return await projects.FirstOrDefaultAsync();
        }
    }

}
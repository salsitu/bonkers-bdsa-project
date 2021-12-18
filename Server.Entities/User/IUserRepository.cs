using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Server.Entities
{
    internal interface IUserRepository
    {
        public Task<(Response, UserDTO)> CreateUserAsync(UserCreateDTO user);
        public Task<UserDTO> GetUserAsync(int userId);
        public Task<UserDTO> GetUserWithEmailAsync(string email);
        public Task<int> GetUserIdWithEmailAsync(string email);
    }
}

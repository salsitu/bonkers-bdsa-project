using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Server.Entities
{
    internal interface IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSupervisor { get; set; }
        public string Email { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBank.Shared
{
    public class Supervisor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Project> CreatedProjects { get; set; }

        public Supervisor() { }
        public Supervisor(int id, string name, List<Project> createdProjects)
        {
            this.Id = id;
            this.Name = name;   
            this.CreatedProjects = createdProjects; 
        }
    }
}

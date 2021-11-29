using System.Collections.Generic;


namespace ProjectBank.Server
{
    public class Supervisor : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Project>? CreatedProjects { get; set; }
    }
}
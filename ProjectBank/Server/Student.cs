

namespace ProjectBank.Server
{
    public class Student : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Project>? Applications { get; set; }
    }
}
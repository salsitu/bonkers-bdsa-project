using System.Collections.Generic;


    public class Supervisor: IUser
{
    public int Id { get; set; }
    public string name { get; set; }
    public ICollection<Project> createdProjects;
}
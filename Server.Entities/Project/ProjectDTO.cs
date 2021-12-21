using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public record ProjectCreateDTO([Required] string Name, string Description, int AuthorId);
    
    public record ProjectDTO(int Id, [Required] string Name, string Description, int AuthorId) : ProjectCreateDTO(Name, Description, AuthorId);
    
    public record SimplifiedProjectDTO(int Id, [Required] string Name);
}
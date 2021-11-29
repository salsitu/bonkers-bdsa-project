using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server
{
    public record ProjectCreateDTO([Required] string Name, string Desc, int AuthorId);
    public record ProjectDTO(int Id, [Required] string Name, string Desc, int AuthorId) : ProjectCreateDTO(Name, Desc, AuthorId);
}
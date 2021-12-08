using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Shared
{
    public record SharedUserCreateDTO([Required] string Name, [Required] bool IsSupervisor, string Email);
    public record SharedUserDTO(int Id, [Required] string Name, [Required] bool IsSupervisor, string Email);
}
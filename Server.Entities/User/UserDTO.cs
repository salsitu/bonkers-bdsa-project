using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public record UserCreateDTO([Required] string Name, [Required] bool IsSupervisor, string Email);
    public record UserDTO(int Id, [Required] string Name, [Required] bool IsSupervisor, string Email);
}
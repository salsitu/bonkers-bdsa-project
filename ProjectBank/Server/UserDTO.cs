using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server
{
    public record UserCreateDTO([Required] string Name, [Required] bool IsSupervisor);
    public record UserDTO(int Id, [Required] string Name, [Required] bool IsSupervisor);
}
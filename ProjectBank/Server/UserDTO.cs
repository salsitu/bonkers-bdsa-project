using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server
{
    public record UserCreateDTO([Required] int UserId, [Required] string Name, [Required] bool IsSupervisor);
}
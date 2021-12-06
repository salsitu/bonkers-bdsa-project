using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public record ViewCreateDTO([Required] int ProjectId, int UserId);
}
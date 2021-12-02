using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server
{
    public record ViewCreateDTO([Required] int ProjectId, int UserId);
}
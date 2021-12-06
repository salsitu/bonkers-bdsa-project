using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public record ApplicantCreateDTO([Required] int ProjectId, int UserId);
}
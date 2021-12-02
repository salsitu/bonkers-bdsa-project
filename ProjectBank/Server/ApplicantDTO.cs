using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server
{
    public record ApplicantCreateDTO([Required] int ProjectId, int UserId);
}
using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public class Applicant : IApplicant
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public int StudentId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public class Project : IProject
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(500)]
        public string Description { get; set; }

        [Required]
        public int AuthorId { get; set; }

        public Project() { }
    }
}
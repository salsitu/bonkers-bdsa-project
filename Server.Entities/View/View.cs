using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public class View : IView
    {
        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int StudentId { get; set; }
    }
}

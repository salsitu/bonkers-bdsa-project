using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server.Entities
{
    public class User : IUser
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public bool IsSupervisor { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
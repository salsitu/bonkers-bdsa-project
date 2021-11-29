using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsSupervisor { get; set; }
    }
}
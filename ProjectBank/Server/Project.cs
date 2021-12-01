using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectBank.Server
{
    public class Project
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(500)]
        public string Desc { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
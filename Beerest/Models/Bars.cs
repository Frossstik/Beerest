using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beerest.Models
{
    public class Bars
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }
    }
}

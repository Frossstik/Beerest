using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Beerest.Models
{
    public class Beers
    {
        [Key]
        [Required]
        public int Id {  get; set; }

        public string? Name { get; set; }

        public string? Country { get; set; }

        public int Volume { get; set; }

        public int VolumeAlc { get; set; }

        public double Price { get; set; }
    }
}

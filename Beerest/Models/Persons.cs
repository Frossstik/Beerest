using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Beerest.Models
{
    public class Persons
    {
        [Key]
        [Required]
        public int Id { get; set; }

        public string? Name { get; set; }

        public int Age { get; set; }

        [ForeignKey("BeerId")]
        public virtual Beers? Beer { get; set; }

        [ForeignKey("BarId")]
        public virtual Bars? Bar { get; set;}

    }
}

using System.ComponentModel.DataAnnotations;

namespace _20240723_SqlDb_Gai.Models
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string? Number { get; set; }
        public string VinCode { get; set; } = "none";
        [Required]
        public string? Model { get; set; }
        [Required]
        public float Volume { get; set; }
    }
}

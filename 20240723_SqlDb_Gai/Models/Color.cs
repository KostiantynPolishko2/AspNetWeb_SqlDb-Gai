using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    [NotMapped]
    public class Color
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public int RAL {  get; set; }
        public string? Type { get; set; }
        public List<Car> Cars { get; set; } = new();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    [NotMapped]
    public class Mark
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Country { get; set; }
        public int PaintThkMin { get; set; }
        public int PaintThkMax { get; set; }
        public List<Car> Cars { get; set; } = new();
    }
}

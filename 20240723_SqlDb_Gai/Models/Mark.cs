using _20240723_SqlDb_Gai.Filter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    //[NotMapped]   
    public class Mark
    {
        [Key]
        public int Id { get; private set; }
        public string? Name { get; private set; }
        public string? Country { get; private set; }
        public int PaintThkMin { get; private set; }
        public int PaintThkMax { get; private set; }
        public List<Car> Cars { get; private set; } = new();
    }
}

using _20240723_SqlDb_Gai.Filter;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    //[NotMapped]
    public class Color
    {
        [Key]
        public int Id { get; private set; }
        public string? Name { get; private set; }
        public int RAL {  get; private set; }
        public string? Type { get; private set; }
        public List<Car> Cars { get; private set; } = new();
    }
}

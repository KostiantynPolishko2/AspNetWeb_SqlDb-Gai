using _20240723_SqlDb_Gai.Swagger;
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    public class Color
    {
        [Key]
        [SwaggerIgnore]
        public int Id { get; private set; }
        public string Name { get; private set; } = null!;
        public int RAL {  get; private set; }
        public string Type { get; private set; } = null!;

        [SwaggerIgnore]
        [NotMapped]
        public List<Car>? Cars { get; private set; } = new();
    }
}

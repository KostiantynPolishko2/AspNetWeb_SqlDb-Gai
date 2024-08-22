using System.ComponentModel.DataAnnotations;

namespace _20240723_SqlDb_Gai.Models
{
    //[NotMapped]
    public class ColorItem
    {
        public string? Name { get; set; }
        public int RAL {  get; set; }
        public string? _Type { get; set; }
    }
}

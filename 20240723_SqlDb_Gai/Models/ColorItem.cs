using Microsoft.EntityFrameworkCore.Query;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    //[NotMapped]
    public class ColorItem
    {
        public string name { get; set; } = null!;
        public int ral {  get; set; }
        public string? type { get; set; }

        public ColorItem() { }

        public ColorItem(Color color)
        {
            this.name = color.Name;
            this.ral = color.RAL;
            this.type = color.Type;
        }
    }
}

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace _20240723_SqlDb_Gai.Models
{
    public class Car
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [Required]
        [DefaultValue(null)]
        public string? Number { get; set; }
        [DefaultValue("none")]
        public string VinCode { get; set; }
        [Required]
        [DefaultValue(null)]
        public string? Model { get; set; }
        [Range(0.01, 6.0, ErrorMessage = "Available value from 0.01 to 6.0")]
        [DefaultValue(0)]
        public float Volume { get; set; }

        [JsonIgnore]
        public int MarkId { get; set; }
        [Required]
        [DefaultValue(null)]
        [JsonIgnore]
        public Mark? _Mark { get; set; }

        [JsonIgnore]
        public int ColorId { get; set; }
        [Required]
        [DefaultValue(null)]
        [JsonIgnore]
        public Color? _Color { get; set; }

        public Car(string? Number, string VinCode, string? Model, float Volume)
        {
            this.Number = Number?.ToUpper();
            this.VinCode = VinCode.ToUpper();
            this.Model = Model?.ToUpper();
            this.Volume = Volume;
        }
    }
}

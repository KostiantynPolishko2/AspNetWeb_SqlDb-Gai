using _20240723_SqlDb_Gai.Filter;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace _20240723_SqlDb_Gai.Models
{
    public class Car
    {
        [Key]
        [SwaggerIgnore]
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

        [SwaggerIgnore]
        public int MarkId { get; set; }
        [Required]
        [DefaultValue(null)]
        [SwaggerIgnore]
        public Mark? _Mark { get; set; }

        [SwaggerIgnore]
        public int ColorId { get; set; }
        [Required]
        [DefaultValue(null)]
        [SwaggerIgnore]
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

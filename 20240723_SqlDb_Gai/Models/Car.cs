using _20240723_SqlDb_Gai.Swagger;
using Asp.Versioning;
using System.ComponentModel.DataAnnotations;

namespace _20240723_SqlDb_Gai.Models
{
    /// <summary>
    /// main entity that depends from Mark and Color
    /// </summary>
    public class Car
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Neccessary registration number")]
        [RegularExpression(@"^[a-zA-Z]{2}\d{4}[a-zA-Z]{2}$", ErrorMessage = "uncorrect format")]
        public string Number { get; set; }
        public string? VinCode { get; set; }
        public string Model { get; set; }
        [Range(0.01, 6.0, ErrorMessage = "Available value from 0.01 to 6.0")]
        public float Volume { get; set; }

        [SwaggerIgnore]
        public int MarkId { get; set; }
        [SwaggerIgnore]
        [Required]
        public Mark _Mark { get; set; } = null!;

        [SwaggerIgnore]
        public int ColorId { get; set; }
        [SwaggerIgnore]
        [Required]
        public Color _Color { get; set; } = null!;

        public Car(string? Number, string VinCode, string? Model, float Volume)
        {
            this.Number = Number!.ToUpper();
            this.VinCode = VinCode.ToUpper();
            this.Model = Model!.ToUpper();
            this.Volume = Volume;
        }

        public Car(Car car, Mark mark, Color color) : this(car.Number, car.VinCode!, car.Model, car.Volume)
        {
            this._Mark = mark;
            this._Color = color;
        }
    }
}

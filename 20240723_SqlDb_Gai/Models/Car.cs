using _20240723_SqlDb_Gai.Filter;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace _20240723_SqlDb_Gai.Models
{
    public class Car
    {
        [SwaggerIgnore]
        public int Id { get; set; }
        public string Number { get; set; }
        public string? VinCode { get; set; }
        public string Model { get; set; }
        [Range(0.01, 6.0, ErrorMessage = "Available value from 0.01 to 6.0")]
        public float Volume { get; set; }

        [SwaggerIgnore]
        public int MarkId { get; set; }
        [SwaggerIgnore]
        public Mark? _Mark { get; set; }

        [SwaggerIgnore]
        public int ColorId { get; set; }
        [SwaggerIgnore]
        public Color? _Color { get; set; }

        public Car(string? Number, string VinCode, string? Model, float Volume)
        {
            this.Number = Number!.ToUpper();
            this.VinCode = VinCode.ToUpper();
            this.Model = Model!.ToUpper();
            this.Volume = Volume;
        }
    }
}

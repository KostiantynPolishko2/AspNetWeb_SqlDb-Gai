using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    //[Table("carsdata")]
    public class Car
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }
        [Required]
        [DefaultValue(null)]
        public string? Number { get; set; }
        [DefaultValue("none")]
        public string VinCode { get; set; } = "none";
        [Required]
        [DefaultValue(null)]
        public string? Model { get; set; }
        [Range(0.01, 6.0, ErrorMessage = "Available value from 0.01 to 6.0")]
        [DefaultValue(0)]
        public float Volume { get; set; }

        public Car(string? Number, string VinCode, string? Model, float Volume) {
            this.Number = Number?.ToUpper();
            this.VinCode = VinCode.ToUpper();
            this.Model = Model?.ToUpper();
            this.Volume = Volume;
        }
    }
}

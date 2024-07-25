using _20240723_SqlDb_Gai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace _20240723_SqlDb_Gai.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private readonly CarContext _carContext;
        private readonly ILogger<CarController> _logger;

        public CarController(ILogger<CarController> logger, CarContext carContext)
        {
            _carContext = carContext;
            _logger = logger;
        }

        private bool IsDbContext() => _carContext.Database.CanConnect();
        private bool IsDbCars() => _carContext.Cars != null ? true : false;
        private bool IsDbMarks() => _carContext.Marks != null ? true : false;
        private Mark? getMark(string markName) => _carContext.Marks.FirstOrDefault(mark => mark.Name.Equals(markName.ToLower()));
        private Color? getColor(string colorName) => _carContext.Colors.FirstOrDefault(color => color.Name.Equals(colorName.ToLower()));
        private Car? getCar(string number) => _carContext.Cars.FirstOrDefault(car => car.Number.Equals(number.ToUpper()));

        [HttpGet(Name = "GetCars")]
        public ActionResult<IEnumerable<Car>> Get() {
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbCars()) return NotFound(new { StatusCode = 400, Message = "no records cars" });

            return _carContext.Cars.ToList();
        }

        [HttpGet("{Number}")]
        public ActionResult<Car> Get([Required] string Number) {
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbCars()) return NotFound(new { StatusCode = 400, Message = "no records cars" });

            Car? car = _carContext.Cars.ToList().Find(car => car.Number.Equals(Number.ToUpper()));

            return  car != null ? car : BadRequest(new { StatusCode = 400, Message = $"{Number} model is absent in db" });
        }

        [HttpPost(Name = "AddCar")]
        public ActionResult<Car> Post([Required] string Number, [Required] string VinCode, [Required] string Model, [Required] float Volume,
            [Required] string markName, [Required] string colorName)
        {
            if (!IsDbContext()) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!IsDbMarks()) return NotFound(new StatusCode(404, $"no records for marks"));

            Mark? mark = getMark(markName);
            Color? color = getColor(colorName);
            Car car = new Car(Number, VinCode, Model, Volume) { MarkId = mark?.Id ?? 0, _Mark = mark, ColorId = color?.Id ?? 0, _Color = color };

            if (ModelState.IsValid)
            {
                _carContext.Cars.Add(car);
                _carContext.SaveChanges();

                return Ok(new StatusCode(201, $"{Number} added to db"));
            }

            return BadRequest(new StatusCode(400, "model is not valid"));
        }

        [HttpPut(Name = "ModifyCar")]
        public IActionResult Put([Required] string Number, string? Model, float? Volume,
            [Required] string markName, [Required] string colorName)
        {
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbMarks()) return NotFound(new { StatusCode = 400, Message = $"no records for marks" });

            Mark? mark = getMark(markName);
            Color? color = getColor(colorName);
            Car? car = getCar(Number);

            if (car is not null) {
                car.Model = Model??car.Model;
                car.Volume = Volume??car.Volume;
                car._Mark = mark??car._Mark;
                car._Color = color??car._Color;
                _carContext.SaveChanges();
            }

            return Ok(new { StatusCode = 200, Message = "modified" });
        }

        [HttpDelete(Name = "DeleteCarId")]
        public ActionResult Delete([Required] int id) {
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbCars()) return NotFound(new { StatusCode = 400, Message = "no records cars" });

            id = Math.Abs(id);
            Car? car = _carContext.Cars.FirstOrDefault(x => x.Id == id);
            if (car != null) { 
                _carContext.Cars.Remove(car);
                _carContext.SaveChanges();
                return Ok(new { StatusCode = 200, _car = car, Message = "deleted" });
            }

            return NotFound(new { StatusCode = 404, _carId = id, Message = "not found" });
        }
    }
}

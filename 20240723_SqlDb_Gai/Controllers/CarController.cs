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
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbMarks()) return NotFound(new { StatusCode = 400, Message = $"no records for marks" });

            Mark? mark = _carContext.Marks.FirstOrDefault(mark => mark.Name.Equals(markName.ToLower()));
            Color? color = _carContext.Colors.FirstOrDefault(color => color.Name.Equals(colorName));

            if (ModelState.IsValid && mark?.Id != null)
            {
                _carContext.Cars.Add(new Car(Number, VinCode, Model, Volume) { MarkId = mark.Id, _Mark = mark, ColorId = color.Id, _Color = color });
                _carContext.SaveChanges();

                return Ok(new { StatusCode = 200, Message = "added to db" });
            }

            return BadRequest(new { StatusCode = 400, Message = "model is not valid" });
        }

        [HttpDelete("{id:int}")]
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

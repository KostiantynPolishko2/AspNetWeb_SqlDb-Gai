using _20240723_SqlDb_Gai.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace _20240723_SqlDb_Gai.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarController : ControllerBase
    {
        private const string patternNumber = @"^[A-Z]{2}\d{4}[A-Z]{2}$";
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
        private Mark? getMark(string markName) => _carContext.Marks.FirstOrDefault(mark => mark.Name!.Equals(markName.ToLower()));
        private Color? getColor(string colorName) => _carContext.Colors.FirstOrDefault(color => color.Name!.Equals(colorName.ToLower()));
        private Car? getCar(string number) => _carContext.Cars.FirstOrDefault(car => car.Number!.Equals(number.ToUpper()));
        private static float getPaintThk(float minThk, float maxThk) => (minThk + maxThk) / 2;
        private bool isNumber(string number) => Regex.IsMatch(number, patternNumber, RegexOptions.IgnoreCase);

        private IActionResult isSaveToDb(string msg = "db saved")
        {
            try
            {
                _carContext.SaveChanges();
                return Ok(new StatusCode(200, msg));
            }
            catch(Exception ex)
            {
                return BadRequest(new StatusCode(400, $"{ex.InnerException!.Message.ToString()}"));
            }
        }

        [HttpGet(Name = "GetCars")]
        public ActionResult<IEnumerable<Car>> Get() {
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbCars()) return NotFound(new { StatusCode = 400, Message = "no records cars" });

            return _carContext.Cars.ToList();
        }

        [HttpGet("Number/{Number}", Name = "GetByNumber")]
        public ActionResult<Car> Get([Required] string Number) {
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbCars()) return NotFound(new { StatusCode = 400, Message = "no records cars" });

            Car? car = _carContext.Cars.ToList().Find(car => car.Number!.Equals(Number.ToUpper()));

            return  car != null ? car : BadRequest(new { StatusCode = 400, Message = $"{Number} model is absent in db" });
        }

        [HttpGet("Mark/{Mark}", Name = "GetByMark")]
        public ActionResult<IEnumerable<CarMarkPaint>> GetCarMarkPaint([Required] string Mark)
        {
            if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbCars()) return NotFound(new { StatusCode = 400, Message = "no records cars" });

            try
            {
                IEnumerable<CarMarkPaint> cars = (from car in _carContext.Cars.Include(car => car._Mark).Include(car => car._Color)
                                                  where car._Mark!.Name!.Equals(Mark.ToLower())
                                                  select
                                                  new CarMarkPaint(car.Number!,
                                                      car._Mark!.Name!, car.Model!,
                                                      getPaintThk(car._Mark.PaintThkMin, car._Mark.PaintThkMax),
                                                      car._Color!.RAL,
                                                      car._Color!.Type!));
                return Ok(cars);
            }
            catch (Exception ex) {
                return NotFound(new StatusCode(404, $"{ex.InnerException!.Message.ToString()}"));
            }
        }


        [HttpPost(Name = "AddCar")]
        public IActionResult Post([Required] string Number, [Required] string VinCode, [Required] string Model, [Required] float Volume,
            [Required] string markName, [Required] string colorName)
        {
            if (!isNumber(Number)) return BadRequest(new StatusCode(400, $"uncorrect format {Number}"));
            else if (!IsDbContext()) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!IsDbMarks()) return NotFound(new StatusCode(404, $"no records for marks"));

            Mark? mark = getMark(markName);
            Color? color = getColor(colorName);
            Car car = new Car(Number, VinCode, Model, Volume) { MarkId = mark?.Id ?? 0, _Mark = mark, ColorId = color?.Id ?? 0, _Color = color };

            if (!ModelState.IsValid)
            {
                BadRequest(new StatusCode(400, "model is not valid"));
            }

            _carContext.Cars.Add(car);
            return isSaveToDb($"{Number} is added to db");
        }

        [HttpPut(Name = "ModifyCar")]
        public IActionResult Put([Required] string Number, string? Model, float? Volume,
            [Required] string markName, [Required] string colorName)
        {
            if (!isNumber(Number)) return BadRequest(new StatusCode(400, $"uncorrect format {Number}"));
            else if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbMarks()) return NotFound(new { StatusCode = 400, Message = $"no records for marks" });


            Car? car = getCar(Number);
            if (car is null) {
                return NotFound(new StatusCode(404, $"no instance by {Number}"));
            }

            Mark? mark = getMark(markName);
            Color? color = getColor(colorName);

            if(mark is null || color is null) return BadRequest(new StatusCode(400, "model of entity is not valid"));

            car!.Model = Model ?? car.Model;
            car.Volume = Volume ?? car.Volume;
            car._Mark = mark!;
            car._Color = color!;

            return isSaveToDb($"{Number} is modified in db");
        }

        [HttpDelete(Name = "DeleteCarId")]
        public IActionResult Delete([Required] string number) {

            if (!isNumber(number)) return BadRequest(new StatusCode(400, $"uncorrect format {number}"));
            else if (!IsDbContext()) return Problem("no connection db");
            else if (!IsDbCars()) return NotFound(new { StatusCode = 400, Message = "no records cars" });

            Car? car = _carContext.Cars.FirstOrDefault(x => x.Number.Equals(number.ToUpper()));
            if (car == null) {
                return NotFound(new StatusCode(404, $"{number} is absent entity in db"));
            }

            _carContext.Cars.Remove(car!);         
            return isSaveToDb($"{number} entity is deleted from db");           
        }
    }
}

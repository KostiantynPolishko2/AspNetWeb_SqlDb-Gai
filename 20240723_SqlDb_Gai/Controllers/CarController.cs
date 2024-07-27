using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Models.Exceptions;
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
                return Ok(new StatusCode200(msg));
            }
            catch(Exception ex)
            {
                return BadRequest(new StatusCode400($"{ex.InnerException!.Message.ToString()}"));
            }
        }

        /// <summary>
        /// Get &lt;Cars> from db
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: No records in table cars of SqlDb carsdata</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [HttpGet(Name = "GetCars")]
        [ProducesResponseType(typeof(IEnumerable<Car>), 200)]
        [ProducesResponseType(typeof(StatusCode), 404)]
        [ProducesResponseType(typeof(StatusCode), 409)]
        public ActionResult<IEnumerable<Car>> Get() {
            if (!IsDbContext()) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!IsDbCars()) return NotFound(new StatusCode(404, $"no records for cars"));

            return Ok(_carContext.Cars);
        }

        /// <summary>
        /// Get instance of Car by its number
        /// </summary>
        /// <param name="Number">Registration Number: AE4000IT</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="400">Failed request: Uncorrect format of number inputed</responce>
        /// <responce code="404">Failed request: No records in table cars of SqlDb carsdata</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [HttpGet("Number/{Number}", Name = "GetByNumber")]
        [ProducesResponseType(typeof(Car), 200)]
        [ProducesResponseType(typeof(StatusCode400), 400)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
        public ActionResult<Car> Get([Required] string Number) {
            if (!isNumber(Number)) return BadRequest(new StatusCode400($"uncorrect format {Number}"));
            else if (!IsDbContext()) return Conflict(new StatusCode409());
            else if (!IsDbCars()) return NotFound(new StatusCode404());

            Car? car = _carContext.Cars.ToList().Find(car => car.Number!.Equals(Number.ToUpper()));

            return  car != null ? Ok(car) : BadRequest(new StatusCode400( $"{Number} model is absent in db"));
        }

        /// <summary>
        /// Get IEnumarable&lt;Car> by it mark
        /// </summary>
        /// <param name="Mark">Car Mark: bmw</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [HttpGet("Mark/{Mark}", Name = "GetByMark")]
        [ProducesResponseType(typeof(IEnumerable<CarMarkPaint>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
        public ActionResult<IEnumerable<CarMarkPaint>> GetCarMarkPaint([Required] string Mark)
        {
            if (!IsDbContext()) return Conflict(new StatusCode409());
            else if (!IsDbCars()) return NotFound(new StatusCode404());

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
                return NotFound(new StatusCode404($"{ex.InnerException!.Message.ToString()}"));
            }
        }


        [HttpPost(Name = "AddCar")]
        public IActionResult Post([Required] string Number, [Required] string VinCode, [Required] string Model, [Required] float Volume,
            [Required] string markName, [Required] string colorName)
        {
            if (!isNumber(Number)) return BadRequest(new StatusCode400($"uncorrect format {Number}"));
            else if (!IsDbContext()) return Conflict(new StatusCode409());
            else if (!IsDbMarks()) return NotFound(new StatusCode404());

            Mark? mark = getMark(markName);
            Color? color = getColor(colorName);
            Car car = new Car(Number, VinCode, Model, Volume) { MarkId = mark?.Id ?? 0, _Mark = mark!, ColorId = color?.Id ?? 0, _Color = color! };

            if (!ModelState.IsValid)
            {
                BadRequest(new StatusCode400("model is not valid"));
            }

            _carContext.Cars.Add(car);
            return isSaveToDb($"{Number} is added to db");
        }

        [HttpPut(Name = "ModifyCar")]
        public IActionResult Put([Required] string Number, string? Model, float? Volume,
            [Required] string markName, [Required] string colorName)
        {
            if (!isNumber(Number)) return BadRequest(new StatusCode400($"uncorrect format {Number}"));
            else if (!IsDbContext()) return Conflict(new StatusCode409());
            else if (!IsDbMarks()) return NotFound(new StatusCode404());


            Car? car = getCar(Number);
            if (car is null) {
                return NotFound(new StatusCode404($"no instance by {Number}"));
            }

            Mark? mark = getMark(markName);
            Color? color = getColor(colorName);

            if(mark is null || color is null) return BadRequest(new StatusCode400("model of entity is not valid"));

            car!.Model = Model ?? car.Model;
            car.Volume = Volume ?? car.Volume;
            car._Mark = mark!;
            car._Color = color!;

            return isSaveToDb($"{Number} is modified in db");
        }

        [HttpDelete(Name = "DeleteCarId")]
        public IActionResult Delete([Required] string number) {

            if (!isNumber(number)) return BadRequest(new StatusCode400($"uncorrect format {number}"));
            else if (!IsDbContext()) return Conflict(new StatusCode409());
            else if (!IsDbCars()) return NotFound(new StatusCode404());

            Car? car = _carContext.Cars.FirstOrDefault(x => x.Number.Equals(number.ToUpper()));
            if (car == null) {
                return NotFound(new StatusCode404($"{number} is absent entity in db"));
            }

            _carContext.Cars.Remove(car!);         
            return isSaveToDb($"{number} entity is deleted from db");           
        }
    }
}

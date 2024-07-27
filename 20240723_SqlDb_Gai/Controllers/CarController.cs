﻿using _20240723_SqlDb_Gai.Models;
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

        private IActionResult isSaveToDb()
        {
            try
            {
                _carContext.SaveChanges();
                return Ok("db saved");
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
                return NotFound(new StatusCode(404, $"mark {Mark} is absent in db"));
            }
        }


        [HttpPost(Name = "AddCar")]
        public IActionResult Post([Required] string Number, [Required] string VinCode, [Required] string Model, [Required] float Volume,
            [Required] string markName, [Required] string colorName)
        {
            if (!IsDbContext()) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!IsDbMarks()) return NotFound(new StatusCode(404, $"no records for marks"));

            Mark? mark = getMark(markName);
            Color? color = getColor(colorName);
            Car car = new Car(Number, VinCode, Model, Volume) { MarkId = mark?.Id ?? 0, _Mark = mark, ColorId = color?.Id ?? 0, _Color = color };

            if (!isNumber(Number))
            {
                return BadRequest(new StatusCode(400, $"uncorrect format {Number}"));
            }
            else if (!ModelState.IsValid)
            {
                BadRequest(new StatusCode(400, "model is not valid"));
            }

            _carContext.Cars.Add(car);
            return isSaveToDb();
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

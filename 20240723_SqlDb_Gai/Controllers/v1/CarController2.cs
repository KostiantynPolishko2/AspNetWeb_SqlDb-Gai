using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Models.Exceptions;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _20240723_SqlDb_Gai.Controllers
{
    [ApiVersion("1.0")]
    public partial class CarController : ControllerBase
    {
        /// <summary>
        /// Get IEnumarable&lt;Car> by it mark
        /// </summary>
        /// <param name="Mark">Car Mark: bmw</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("1.0")]
        [HttpGet("Mark/{Mark}", Name = "GetCarMarkPaint")]
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

        /// <summary>
        /// Add to db entity Car filled required fields
        /// </summary>
        /// <param name="Number">Registration Number: AE4000IT</param>
        /// <param name="VinCode">Unique code: 5UXWX9C34H0W67034</param>
        /// <param name="Model">Name of model</param>
        /// <param name="Volume">Engine volume</param>
        /// <param name="markName">Name of mark</param>
        /// <param name="colorName">Painted color: red</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="400">Failed request: Uncorrect format of number inputed</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("1.0")]
        [HttpPost(Name = "AddCar")]
        [ProducesResponseType(typeof(StatusCode200), 200)]
        [ProducesResponseType(typeof(StatusCode400), 400)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Number">Registration Number: AE4000IT</param>
        /// <param name="Model">Name of model</param>
        /// <param name="Volume">Engine volume</param>
        /// <param name="markName">Name of mark</param>
        /// <param name="colorName">Painted color: red</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="400">Failed request: Uncorrect format of number inputed</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("1.0")]
        [HttpPut(Name = "ModifyCar")]
        [ProducesResponseType(typeof(StatusCode200), 200)]
        [ProducesResponseType(typeof(StatusCode400), 400)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
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
    }
}

using _20240723_SqlDb_Gai.Database;
using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Models.Exceptions;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace _20240723_SqlDb_Gai.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public partial class CarController : ControllerBase
    {
        private readonly CarContext carContext;
        private readonly ILogger<CarController> logger;

        public CarController(ILogger<CarController> logger, CarContext carContext)
        {
            this.carContext = carContext;
            this.logger = logger;
        }

        private Mark? getMark(string markName) => this.carContext.Marks.FirstOrDefault(mark => mark.Name!.Equals(markName.ToLower()));
        private Color? getColor(string colorName) => this.carContext.Colors.FirstOrDefault(color => color.Name!.Equals(colorName.ToLower()));
        private Car? getCar(string number) => this.carContext.Cars.FirstOrDefault(car => car.Number!.Equals(number.ToUpper()));

        /// <summary>
        /// Get &lt;Cars> from db
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: No records in table cars of SqlDb carsdata</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("1.0")]
        [HttpGet(Name = "GetCars")]
        [ProducesResponseType(typeof(IEnumerable<Car>), 200)]
        [ProducesResponseType(typeof(StatusCode), 404)]
        [ProducesResponseType(typeof(StatusCode), 409)]
        public ActionResult<IEnumerable<Car>> Get() {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!DbVarification.IsDbCars(carContext)) return NotFound(new StatusCode(404, $"no records for cars"));

            return Ok(this.carContext.Cars);
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
        [MapToApiVersion("1.0")]
        [HttpGet("Number/{Number}", Name = "GetByNumber")]
        [ProducesResponseType(typeof(Car), 200)]
        [ProducesResponseType(typeof(StatusCode400), 400)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
        public ActionResult<Car> Get([Required] string Number) {
            if (!DbVarification.isNumber(Number)) return BadRequest(new StatusCode400($"uncorrect format {Number}"));
            else if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode409());
            else if (!DbVarification.IsDbCars(carContext)) return NotFound(new StatusCode404());

            Car? car = this.carContext.Cars.FirstOrDefault(car => car.Number!.Equals(Number.ToUpper()));

            //IEnumerable<Car> cars = (from car in _carContext.Cars.Include(c => c._Mark).Include(c => c._Color)
            //            where car.Number.Equals(Number.ToUpper())
            //            select new Car(car, car._Mark, car._Color));

            return  car != null ? Ok(car) : BadRequest(new StatusCode400( $"{Number} model is absent in db"));
        }

        /// <summary>
        /// Delete from db entity Car by registration Number
        /// </summary>
        /// <param name="number">Registration Number: AE4000IT</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="400">Failed request: Uncorrect format of number inputed</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("1.0")]
        [HttpDelete(Name = "DeleteCarId")]
        [ProducesResponseType(typeof(StatusCode200), 200)]
        [ProducesResponseType(typeof(StatusCode400), 400)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
        public IActionResult Delete([Required] string number) {

            if (!DbVarification.isNumber(number)) return BadRequest(new StatusCode400($"uncorrect format {number}"));
            else if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode409());
            else if (!DbVarification.IsDbCars(carContext)) return NotFound(new StatusCode404());

            Car? car = this.carContext.Cars.FirstOrDefault(x => x.Number.Equals(number.ToUpper()));
            if (car == null) {
                return NotFound(new StatusCode404($"{number} is absent entity in db"));
            }

            this.carContext.Cars.Remove(car!);         
            return Ok(DbVarification.isSaveToDb(this.carContext, $"{number} entity is deleted from db"));           
        }
    }
}

using _20240723_SqlDb_Gai.Database;
using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Models.Exceptions;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace _20240723_SqlDb_Gai.Controllers
{
    [ApiVersion("3.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly CarContext carContext;
        private readonly ILogger<ColorController> logger;

        public ColorController(ILogger<ColorController> logger, CarContext carContext)
        {
            this.carContext = carContext;
            this.logger = logger;
        }

        /// <summary>
        /// Get IEnumarable&lt;ColorItems>
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("3.0")]
        [HttpGet("ColorItems", Name = "GetColorItems")]
        [ProducesResponseType(typeof(IEnumerable<ColorItem>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]

        public ActionResult<IEnumerable<ColorItem>> getColorItems()
        {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode409());
            else if (!DbVarification.IsDbColors(carContext)) return NotFound(new StatusCode404());

            IEnumerable<ColorItem> colorItems = (from color in this.carContext.Colors
                                             select
                                             new ColorItem() { Name = color.Name, RAL = color.RAL, _Type = color.Type });

            return Ok(colorItems);
        }

        /// <summary>
        /// Get IEnumarable&lt;Colors>
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("3.0")]
        [HttpGet("Colors", Name = "GetColors")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]

        public ActionResult<IEnumerable<string>> getColors()
        {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!DbVarification.IsDbColors(carContext)) return NotFound(new StatusCode(404, $"no records for colors"));

            List<string> colors = new List<string>() { };
            this.carContext.Colors.ToList().ForEach(c => colors.Add(c.Name!));

            return Ok(colors);
        }

        /// <summary>
        /// Get ColorItem
        /// </summary>
        /// <remarks>
        ///     example of request
        ///     
        ///     GET / Todo
        ///     https://localhost:7133/api/v3/Color/yellow?colorName=yellow%20hues
        ///     
        /// </remarks>
        /// <param name="colorName">Color Name from SQL DB</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("3.0")]
        [HttpGet("{colorName}", Name = "GetColor")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]

        public ActionResult<Color> getColorItem([FromQuery] string colorName)
        {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!DbVarification.IsDbColors(carContext)) return NotFound(new StatusCode(404, $"no records for colors"));

            Color? color = this.carContext.Colors.FirstOrDefault(x => x.Name.Equals(colorName.ToLower()));
            if (color == null)
            {
                return NotFound(new StatusCode404($"{colorName} is absent entity in db"));
            }

            return Ok(color);
        }
    }
}

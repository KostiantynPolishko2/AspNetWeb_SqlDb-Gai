using _20240723_SqlDb_Gai.Database;
using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Models.Exceptions;
using Asp.Versioning;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
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
        private const string color = "yellow";

        public ColorController(ILogger<ColorController> logger, CarContext carContext)
        {
            this.carContext = carContext;
            this.logger = logger;
        }

        private ColorItem? _getColorItem(string name) => this.carContext.ColorItems.FirstOrDefault(colorItem => colorItem.name.Equals(name.ToLower()));

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
                                             new ColorItem() { name = color.Name, ral = color.RAL, type = color.Type });

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
        /// getColorItem([FromQuery] string colorName)
        /// </summary>
        /// <remarks>
        ///     example of request
        ///     
        ///     GET / Todo
        ///     https://localhost:7133/api/v3/Color/yellow?colorName=yellow%20hues
        ///     
        /// </remarks>
        /// <param name="colorName">[FromQuery]</param>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("3.0")]
        [HttpGet("{index:int}", Name = "GetColor")]
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

        /// <summary>
        /// post([FromRoute] ColorItem colorItem)
        /// </summary>
        /// <remarks>
        ///     example of request
        ///     
        ///     POST / Todo
        ///     https://localhost:7133/api/v3/Color/light%20gray/40/nitro
        ///     
        /// </remarks>
        /// <param name="colorItem">[FromRoute]</param>
        /// <returns></returns>
        /// <responce code="201">Request successful: </responce>
        /// <responce code="404">Request failed: Not found data</responce>
        /// <responce code="409">Request failed: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("3.0")]
        [HttpPost("{name}/{ral:int}/{type?}", Name = "AddColor")]
        [ProducesResponseType(typeof(StatusCode201), 201)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]

        public IActionResult addColor([FromRoute] ColorItem colorItem)
        {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!DbVarification.IsDbColors(carContext)) return NotFound(new StatusCode(404, $"no records for colors"));

            if (!ModelState.IsValid)
            {
                BadRequest(new StatusCode400("instance is not valid"));
            }

            this.carContext.ColorItems.Add(colorItem);
            return Ok(DbVarification.isSaveToDb(this.carContext, $"{colorItem.name} is added to db"));
        }

        /// <summary>
        /// put([FromBody] ColorItem colorItem)
        /// </summary>
        /// <remarks>
        ///     example of request
        ///     
        ///     POST / Todo
        ///     https://localhost:7133/api/v3/Color/yellow/
        ///     
        /// </remarks>
        /// <param name="name">[FromQuery]</param>
        /// <param name="colorItem">[FromBody]</param>
        /// <returns></returns>
        /// <responce code="201">Request successful: </responce>
        /// <responce code="404">Request failed: Not found data</responce>
        /// <responce code="409">Request failed: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("3.0")]
        [HttpPut(Name = "UpdateColor")]
        [ProducesResponseType(typeof(StatusCode201), 201)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]

        public IActionResult UpdateColor([FromQuery] string name, [FromBody] ColorItem colorItem)
        {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!DbVarification.IsDbColors(carContext)) return NotFound(new StatusCode(404, $"no records for colors"));

            if (!ModelState.IsValid)
            {
                BadRequest(new StatusCode400("instance is not valid"));
            }

            ColorItem? oldColorItem = _getColorItem(name.ToLower());
            if (oldColorItem is null) return BadRequest(new StatusCode400("model of entity is not valid"));

            oldColorItem.ral = colorItem.ral == 0 ? oldColorItem.ral : colorItem.ral;
            oldColorItem.type = colorItem.type == string.Empty ? oldColorItem.type : colorItem.type;
            carContext.ColorItems.Update(oldColorItem);

            return Ok(DbVarification.isSaveToDb(this.carContext, $"{name} is added to db"));
        }

        /// <summary>
        /// put([FromBody] ColorItem colorItem)
        /// </summary>
        /// <remarks>
        ///     example of request
        ///     
        ///     POST / Todo
        ///     https://localhost:7133/api/v3/Color/yellow/
        ///     
        /// </remarks>
        /// <param name="name">[FromQuery]</param>
        /// <param name="patchColorItem">[FromBody]</param>
        /// <returns></returns>
        /// <responce code="201">Request successful: </responce>
        /// <responce code="404">Request failed: Not found data</responce>
        /// <responce code="409">Request failed: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("3.0")]
        [HttpPatch(Name = "UpdateColorPatch")]
        [ProducesResponseType(typeof(StatusCode201), 201)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]

        public IActionResult UpdateColorPatch([FromQuery] string name, [FromBody] JsonPatchDocument<ColorItem> patchColorItem)
        {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode(409, "no connection db"));
            else if (!DbVarification.IsDbColors(carContext)) return NotFound(new StatusCode(404, $"no records for colors"));

            ColorItem? colorItemDb = _getColorItem(name.ToLower());
            if (colorItemDb is null) return BadRequest(new StatusCode400("model of entity is not valid"));

            patchColorItem.ApplyTo(colorItemDb, ModelState);

            if (!TryValidateModel(colorItemDb))
            {
                BadRequest(new StatusCode400("instance is not valid"));
            }

            carContext.ColorItems.Update(colorItemDb);

            return Ok(DbVarification.isSaveToDb(this.carContext, $"{name} is added to db"));
        }
    }
}

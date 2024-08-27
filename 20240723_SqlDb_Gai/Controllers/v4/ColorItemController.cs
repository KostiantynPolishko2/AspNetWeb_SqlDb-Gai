using _20240723_SqlDb_Gai.Database;
using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Models.Exceptions;
using _20240723_SqlDb_Gai.Repository;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace _20240723_SqlDb_Gai.Controllers.v4
{
    [ApiVersion("4.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ColorItemController : ControllerBase
    {
        private readonly IColorItemRepository colorItemRepository;
        private readonly ILogger<ColorItemController> logger;

        public ColorItemController(IColorItemRepository colorItemRepository, ILogger<ColorItemController> logger)
        {
            this.colorItemRepository = colorItemRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Get IEnumarable&lt;ColorItems>
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("4.0")]
        [HttpGet("ColorItems", Name = "GetColorItems")]
        [ProducesResponseType(typeof(IEnumerable<ColorItem>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]

        public ActionResult<IEnumerable<ColorItem>> getColorItems()
        {
            if (!DbVarification.IsDbContext(colorItemRepository.getContext())) return Conflict(new StatusCode409());
            else if (!DbVarification.IsDbColors(colorItemRepository.getContext())) return NotFound(new StatusCode404());

            return Ok(colorItemRepository.getAllColorItems());
        }
    }
}

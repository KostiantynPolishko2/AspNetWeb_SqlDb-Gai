using _20240723_SqlDb_Gai.Database;
using _20240723_SqlDb_Gai.Models.Exceptions;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using _20240723_SqlDb_Gai.Controllers;

namespace _20240723_SqlDb_Gai.Controllers.v2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MarkController : ControllerBase
    {
        private readonly CarContext carContext;
        private readonly ILogger<MarkController> logger;

        public MarkController(ILogger<MarkController> logger, CarContext carContext)
        {
            this.carContext = carContext;
            this.logger = logger;
        }

        /// <summary>
        /// Get list of name marks car from db
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("2.0")]
        [HttpGet("Marks", Name = "GetMarks")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
        public ActionResult<IEnumerable<string>> GetMarks()
        {
            if (!DbVarification.IsDbContext(carContext)) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!DbVarification.IsDbMarks(carContext)) return NotFound(new StatusCode(404, $"no records for marks"));

            List<string> marks = new List<string>() { };
            carContext.Marks.ToList().ForEach(m => marks.Add(m.Name!));

            return Ok(marks);
        }
    }
}

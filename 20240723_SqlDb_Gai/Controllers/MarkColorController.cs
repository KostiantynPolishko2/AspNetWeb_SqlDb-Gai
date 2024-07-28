using _20240723_SqlDb_Gai.Models;
using _20240723_SqlDb_Gai.Models.Exceptions;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace _20240723_SqlDb_Gai.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class MarkColorController : ControllerBase
    {
        private readonly CarContext carContext;
        private readonly ILogger<MarkColorController> logger;

        public MarkColorController(ILogger<MarkColorController> logger, CarContext carContext)
        {
            this.carContext = carContext;
            this.logger = logger;
        }

        private bool IsDbContext() => this.carContext.Database.CanConnect();
        private bool IsDbMarks() => this.carContext.Marks != null ? true : false;
        private bool IsDbColors() => this.carContext.Colors != null ? true : false;

        /// <summary>
        /// Get list of name marks car from db
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("1.0")]
        [HttpGet("ListMarks", Name = "GetMarks")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
        public ActionResult<IEnumerable<string>> GetMarks() {
            if (!IsDbContext()) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!IsDbMarks()) return NotFound(new StatusCode(404, $"no records for marks"));

            List<string> nameMarks = new List<string>() { };
            carContext.Marks.ToList().ForEach(m => nameMarks.Add(m.Name!));
            
            return Ok(nameMarks);
        }

        /// <summary>
        /// Get list of name colors car from db
        /// </summary>
        /// <returns></returns>
        /// <responce code="200">Successful request fulfillment</responce>
        /// <responce code="404">Failed request: Not found data</responce>
        /// <responce code="409">Failed request: No connection to SqlDb carsdata</responce>
        [MapToApiVersion("1.0")]
        [HttpGet("ListColors", Name = "GetColors")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        [ProducesResponseType(typeof(StatusCode404), 404)]
        [ProducesResponseType(typeof(StatusCode409), 409)]
        public ActionResult<IEnumerable<string>> GetColors()
        {
            if (!IsDbContext()) return Conflict(new StatusCode(409, "no connectio db"));
            else if (!IsDbColors()) return NotFound(new StatusCode(404, $"no records for colors"));

            List<string> nameColors = new List<string>() { };
            carContext.Colors.ToList().ForEach(c => nameColors.Add(c.Name!));

            return Ok(nameColors);
        }
    }
}

using _20240723_SqlDb_Gai.Filter;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models.Exceptions
{
    [NotMapped]
    [SwaggerIgnore]
    public class StatusCode409 : StatusCode
    {
        public StatusCode409() : base(409, "no connection to db") { }
        public StatusCode409(string msg) : base(409, msg) { }
        public StatusCode409(int statusCode, string msg) : base(statusCode, msg) { }
    }
}

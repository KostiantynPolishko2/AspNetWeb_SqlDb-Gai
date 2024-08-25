using _20240723_SqlDb_Gai.Swagger;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models.Exceptions
{
    [NotMapped]
    [SwaggerIgnore]
    public class StatusCode201 : StatusCode
    {
        public StatusCode201() : base(201, "item was added to db") { }
        public StatusCode201(string msg) : base(201, msg) { }
        public StatusCode201(int statusCode, string msg) : base(statusCode, msg) { }
    }
}

using _20240723_SqlDb_Gai.Filter;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models.Exceptions
{
    [NotMapped]
    [SwaggerIgnore]
    public class StatusCode404 : StatusCode
    {
        public StatusCode404() : base(404, "records are absent in db") { }
        public StatusCode404(string msg) : base(404, msg) { }
        public StatusCode404(int statusCode, string msg) : base(statusCode, msg) { }
    }
}

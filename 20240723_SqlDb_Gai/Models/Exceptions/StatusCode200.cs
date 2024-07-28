using _20240723_SqlDb_Gai.Filter;
using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models.Exceptions
{
    [NotMapped]
    [SwaggerIgnore]
    public class StatusCode200 : StatusCode
    {
        public StatusCode200() : base(200, "request done") { }
        public StatusCode200(string msg) : base(200, msg) { }
        public StatusCode200(int statusCode, string msg) : base(statusCode, msg) { }
    }
}

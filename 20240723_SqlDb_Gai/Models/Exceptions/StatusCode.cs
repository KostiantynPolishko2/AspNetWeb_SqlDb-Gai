using System.ComponentModel.DataAnnotations.Schema;
using _20240723_SqlDb_Gai.Filter;

namespace _20240723_SqlDb_Gai.Models.Exceptions
{
    [NotMapped]
    [SwaggerIgnore]
    public class StatusCode
    {
        public int code { get; }
        public string message { get; } = null!;

        public StatusCode(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}
